import express from 'express';
import cors from 'cors';
import bodyParser from 'body-parser';
import {courses, users, testResults, monthlyActivity, userProgress, detailedProgress} from "./data.js";

const app = express();
const PORT = 5000;

app.use(cors());
app.use(bodyParser.json());

app.get('/api/courses/structure', (req, res) => {
    try {
        res.json(courses);
    } catch (err) {
        console.error('Error fetching courses structure:', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.get('/api/progress/all', (req, res) => {
    try {
        const result = userProgress.map(progress => {
            const user = users.find(u => u.id === progress.userId);
            return {
                ...progress,
                user: {
                    id: user?.id,
                    name: user?.name,
                    username: user?.username,
                    email: user?.email,
                    avatar: user?.avatar
                }
            };
        });
        res.json(result);
    } catch (err) {
        console.error('Error fetching all progress:', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.get('/api/progress/user/:userId', (req, res) => {
    try {
        const userId = parseInt(req.params.userId);
        const progress = detailedProgress.find(p => p.userId === userId);

        if (!progress) {
            return res.status(404).json({ error: 'Progress not found' });
        }

        const user = users.find(u => u.id === userId);
        const course = courses.find(c => c.id === progress.courseId);

        res.json({
            ...progress,
            user: {
                id: user?.id,
                name: user?.name,
                email: user?.email,
                avatar: user?.avatar
            },
            courseName: course?.title
        });
    } catch (err) {
        console.error('Error fetching user progress:', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.post('/api/progress/lesson', (req, res) => {
    try {
        const { userId, courseId, moduleId, lessonId } = req.body;

        // Находим или создаем запись о прогрессе пользователя
        let progress = userProgress.find(p =>
            p.userId === userId && p.courseId === courseId
        );

        if (!progress) {
            progress = {
                userId,
                courseId,
                progress: 0,
                lastActivity: new Date().toISOString(),
                modules: {}
            };
            userProgress.push(progress);
        }

        if (!progress.modules[moduleId]) {
            progress.modules[moduleId] = {
                progress: 0,
                lessonsCompleted: 0,
                testsCompleted: 0,
                lastActivity: new Date().toISOString()
            };
        }

        const course = courses.find(c => c.id === courseId);
        const module = course?.modules.find(m => m.id === moduleId);

        if (!course || !module) {
            return res.status(404).json({ error: 'Course or module not found' });
        }

        const detailed = detailedProgress.find(p =>
            p.userId === userId && p.courseId === courseId
        );

        if (!detailed) {
            detailedProgress.push({
                userId,
                courseId,
                detailedProgress: {
                    modules: [
                        {
                            id: moduleId,
                            name: module.title,
                            progress: 0,
                            lessons: [],
                            tests: []
                        }
                    ]
                }
            });
        }

        progress.lastActivity = new Date().toISOString();
        progress.modules[moduleId].lastActivity = new Date().toISOString();

        res.json(progress);
    } catch (err) {
        console.error('Error updating lesson progress:', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.post('/api/progress/test', (req, res) => {
    try {
        const { userId, courseId, moduleId, testId, score, answers } = req.body;

        res.json({ success: true });
    } catch (err) {
        console.error('Error updating test progress:', err);
        res.status(500).json({ error: 'Internal server error' });
    }
});

app.get('/api/analytics/overall', (req, res) => {
    const totalStudents = users.length;
    const activeStudents = users.filter(u =>
        new Date(u.lastActive) > new Date(Date.now() - 30*24*60*60*1000)
    ).length;

    const scores = testResults.reduce((acc, result) => {
        if (result.score > 90) acc.excellent++;
        else if (result.score >= 70) acc.good++;
        else if (result.score >= 50) acc.average++;
        else acc.weak++;
        return acc;
    }, { excellent: 0, good: 0, average: 0, weak: 0 });

    res.json({
        total: totalStudents,
        active: activeStudents,
        ...scores
    });
});

app.get('/api/analytics/individual', (req, res) => {
    const studentsWithStats = users
        .map(user => {
            const userResults = testResults.filter(r => r.userId === user.id);
            const averageScore = userResults.length > 0
                ? userResults.reduce((sum, r) => sum + r.score, 0) / userResults.length
                : 0;

            return {
                ...user,
                stats: {
                    completedCourses: new Set(userResults.map(r => r.courseId)).size,
                    averageScore,
                    lastActivity: user.lastActive
                }
            };
        });

    res.json(studentsWithStats);
});

app.get('/api/analytics/activity', (req, res) => {
    const { year = '2025' } = req.query;
    res.json(monthlyActivity[year] || []);
});

app.get('/api/analytics/course/:courseId', (req, res) => {
    const courseResults = testResults.filter(r => r.courseId === req.params.courseId);

    if (courseResults.length === 0) {
        return res.status(404).json({ error: 'No data for this course' });
    }

    const scores = courseResults.reduce((acc, result) => {
        if (result.score > 90) acc.excellent++;
        else if (result.score >= 70) acc.good++;
        else if (result.score >= 50) acc.average++;
        else acc.weak++;
        return acc;
    }, { excellent: 0, good: 0, average: 0, weak: 0 });

    res.json({
        courseId: req.params.courseId,
        totalAttempts: courseResults.length,
        averageScore: courseResults.reduce((sum, r) => sum + r.score, 0) / courseResults.length,
        ...scores
    });
});
app.post('/api/test-results', (req, res) => {
    const { userId, courseId, moduleId, lessonId, score } = req.body;

    const user = users.find(u => u.id === userId);
    if (!user) {
        return res.status(404).json({ error: 'User not found' });
    }

    const userCourse = user.courses?.find(c => c.id === courseId);
    if (userCourse) {
        userCourse.score = Math.max(userCourse.score || 0, score);
    } else {
        user.courses = user.courses || [];
        user.courses.push({
            id: courseId,
            name: courses.find(c => c.id === courseId)?.title || 'Unknown',
            score,
            completed: true
        });
    }

    const newResult = {
        userId,
        courseId,
        moduleId,
        lessonId,
        score,
        date: new Date().toISOString()
    };
    testResults.push(newResult);

    res.status(201).json(newResult);
});
app.route('/api/login').post(
    (req, res) => {
        console.log(req.body);
});
app.route('/api/users')
    .get((req, res) => {
        res.json(users);
    })
    .post((req, res) => {
        const newUser = {
            id: users.length > 0 ? Math.max(...users.map(u => u.id)) + 1 : 1,
            ...req.body
        };
        users.push(newUser);
        res.status(201).json(newUser);
        console.log(users);
    });

app.route('/api/users/:id')
    .patch((req, res) => {
        const id = parseInt(req.params.id);
        const userIndex = users.findIndex(u => u.id === id);

        if (userIndex === -1) {
            return res.status(404).json({ error: 'User not found' });
        }

        users[userIndex] = { ...users[userIndex], ...req.body };
        res.json(users[userIndex]);
    })
    .delete((req, res) => {
        const id = parseInt(req.params.id);
        const userIndex = users.findIndex(u => u.id === id);

        if (userIndex === -1) {
            return res.status(404).json({ error: 'User not found' });
        }

        users = users.filter(u => u.id !== id);
        res.status(204).end();
    });

app.route('/api/courses')
    .get((req, res) => {
        res.json(courses);
        console.log(courses);

    })
    .post((req, res) => {
        const newCourse = {
            id: Date.now().toString(),
            ...req.body,
            modules: req.body.modules || []
        };
        courses.push(newCourse);
        res.status(201).json(newCourse);
        console.log(courses);
    });

app.route('/api/courses/:id')
    .get((req, res) => {
        const course = courses.find(c => c.id === req.params.id);
        if (!course) return res.status(404).json({ error: 'Course not found' });
        res.json(course);
        console.log(courses);

    })
    .patch((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.id);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }
        courses[courseIndex] = { ...courses[courseIndex], ...req.body };
        res.json(courses[courseIndex]);
        console.log(courses);

    })
    .delete((req, res) => {
        courses = courses.filter(c => c.id !== req.params.id);
        res.status(204).end();
        console.log(courses);

    });

// Роуты для модулей
app.route('/api/courses/:courseId/modules')
    .post((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        const newModule = {
            id: Date.now().toString(),
            ...req.body,
            lessons: []
        };

        courses[courseIndex].modules.push(newModule);
        res.status(201).json(newModule);
    });

app.route('/api/courses/:courseId/modules/:moduleId')
    .patch((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        const moduleIndex = courses[courseIndex].modules.findIndex(
            m => m.id === req.params.moduleId
        );

        if (moduleIndex === -1) {
            return res.status(404).json({ error: 'Module not found' });
        }

        courses[courseIndex].modules[moduleIndex] = {
            ...courses[courseIndex].modules[moduleIndex],
            ...req.body
        };

        res.json(courses[courseIndex].modules[moduleIndex]);
    })
    .delete((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        courses[courseIndex].modules = courses[courseIndex].modules.filter(
            m => m.id !== req.params.moduleId
        );

        res.status(204).end();
    });

app.route('/api/courses/:courseId/modules/:moduleId/lessons')
    .post((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        const moduleIndex = courses[courseIndex].modules.findIndex(
            m => m.id === req.params.moduleId
        );

        if (moduleIndex === -1) {
            return res.status(404).json({ error: 'Module not found' });
        }

        const newLesson = {
            id: Date.now().toString(),
            ...req.body
        };

        courses[courseIndex].modules[moduleIndex].lessons.push(newLesson);
        res.status(201).json(newLesson);
    });

app.route('/api/courses/:courseId/modules/:moduleId/lessons/:lessonId')
    .patch((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        const moduleIndex = courses[courseIndex].modules.findIndex(
            m => m.id === req.params.moduleId
        );

        if (moduleIndex === -1) {
            return res.status(404).json({ error: 'Module not found' });
        }

        const lessonIndex = courses[courseIndex].modules[moduleIndex].lessons.findIndex(
            l => l.id === req.params.lessonId
        );

        if (lessonIndex === -1) {
            return res.status(404).json({ error: 'Lesson not found' });
        }

        courses[courseIndex].modules[moduleIndex].lessons[lessonIndex] = {
            ...courses[courseIndex].modules[moduleIndex].lessons[lessonIndex],
            ...req.body
        };

        res.json(courses[courseIndex].modules[moduleIndex].lessons[lessonIndex]);
    })
    .delete((req, res) => {
        const courseIndex = courses.findIndex(c => c.id === req.params.courseId);
        if (courseIndex === -1) {
            return res.status(404).json({ error: 'Course not found' });
        }

        const moduleIndex = courses[courseIndex].modules.findIndex(
            m => m.id === req.params.moduleId
        );

        if (moduleIndex === -1) {
            return res.status(404).json({ error: 'Module not found' });
        }

        courses[courseIndex].modules[moduleIndex].lessons =
            courses[courseIndex].modules[moduleIndex].lessons.filter(
                l => l.id !== req.params.lessonId
            );

        res.status(204).end();
    });

app.use((err, req, res, next) => {
    console.error(err.stack);
    res.status(500).json({ error: 'Something went wrong!' });
});
app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});