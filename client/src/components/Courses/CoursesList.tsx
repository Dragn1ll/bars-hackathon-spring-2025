import React, { useState, useEffect } from 'react';
import {
    Box,
    Typography,
    List,
    ListItem,
    ListItemText,
    Collapse,
    IconButton,
    Button,
    TextField,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Chip,
    Tooltip,
    CircularProgress,
    Alert, Checkbox
} from '@mui/material';
import {
    ExpandMore,
    ExpandLess,
    Add,
    Edit,
    Delete,
    VideoLibrary,
    Article,
    Quiz
} from '@mui/icons-material';
import useCourses from '../../hooks/useCourses';
import {
    ICourse,
    ILesson,
    IModule,
    ILessonContent,
    IQuizQuestion,
    ITextLessonContent,
    IVideoLessonContent, IQuizLessonContent, ICoursesListProps
} from "../../types/types";
import ConfirmDialog from "./ConfirmDialog";
import MySpinner from "../UI/spinner/MySpinner";

const CoursesList: React.FC<ICoursesListProps> = ({ courses, onUpdateCourse }) => {
    const {
        loading,
        error,
        fetchCourses,
        createCourse,
        updateCourse,
        deleteCourse,
        createModule,
        updateModule,
        deleteModule,
        createLesson,
        updateLesson,
        deleteLesson,
        resetError
    } = useCourses();
    const [deleteDialog, setDeleteDialog] = useState({
        open: false,
        type: '',
        data: {
            courseId: '',
            moduleId: '',
            lessonId: ''
        },
        title: '',
        message: ''
    });
    const [expandedCourses, setExpandedCourses] = useState<Record<string, boolean>>({});
    const [expandedModules, setExpandedModules] = useState<Record<string, boolean>>({});
    const [dialogOpen, setDialogOpen] = useState(false);
    const [currentDialog, setCurrentDialog] = useState<'course' | 'module' | 'lesson' | null>(null);
    const [currentContext, setCurrentContext] = useState<{
        courseId?: string;
        moduleId?: string;
    } | null>(null);
    const [editItem, setEditItem] = useState<ICourse | IModule | ILesson | null>(null);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [contentType, setContentType] = useState<'text' | 'video' | 'quiz'>('text');
    const [content, setContent] = useState('');
    const [quizQuestions, setQuizQuestions] = useState<IQuizQuestion[]>([]);
    const [deleteError, setDeleteError] = useState('');

    useEffect(() => {
        const loadCourses = async () => {
            try {
                const data = await fetchCourses();
                onUpdateCourse(data);
            } catch (err) {
                console.error('Failed to load courses:', err);
            }
        };
        loadCourses();
    }, []);

    const handleDeleteClick = (type: 'course' | 'module' | 'lesson', ids: { courseId?: string, moduleId?: string, lessonId?: string }) => {
        const messages = {
            course: 'Вы уверены, что хотите удалить этот курс? Это действие нельзя отменить.',
            module: 'Вы уверены, что хотите удалить этот модуль? Все уроки в нем будут удалены.',
            lesson: 'Вы уверены, что хотите удалить этот урок?'
        };

        setDeleteDialog({
            open: true,
            type,
            data: {
                courseId: ids.courseId || '',
                moduleId: ids.moduleId || '',
                lessonId: ids.lessonId || ''
            },
            title: `Удаление ${type === 'course' ? 'курса' : type === 'module' ? 'модуля' : 'урока'}`,
            message: messages[type]
        });
    };

    const handleConfirmDelete = async () => {
        try {
            // setLoading(true);
            const { courseId, moduleId, lessonId } = deleteDialog.data;

            switch (deleteDialog.type) {
                case 'course':
                    await deleteCourse(courseId);
                    break;
                case 'module':
                    await deleteModule(courseId, moduleId);
                    break;
                case 'lesson':
                    await deleteLesson(courseId, moduleId, lessonId);
                    break;
            }

            const updatedCourses = await fetchCourses();
            onUpdateCourse(updatedCourses);
        } catch (err) {
            setDeleteError('Не удалось выполнить удаление');
            console.error(err);
        }
        finally {
            setDeleteDialog({ ...deleteDialog, open: false });
            // setLoading(false);
        }
    };

    const toggleCourse = (courseId: string) => {
        setExpandedCourses(prev => ({
            ...prev,
            [courseId]: !prev[courseId]
        }));
    };

    const toggleModule = (moduleId: string) => {
        setExpandedModules(prev => ({
            ...prev,
            [moduleId]: !prev[moduleId]
        }));
    };

    const openDialog = (
        type: 'course' | 'module' | 'lesson',
        context?: { courseId?: string; moduleId?: string },
        itemToEdit?: ICourse | IModule | ILesson
    ) => {
        setCurrentDialog(type);
        setCurrentContext(context || null);
        setEditItem(itemToEdit || null);

        if (itemToEdit) {
            setTitle('title' in itemToEdit ? itemToEdit.title : '');

            if (type === 'course') {
                setDescription((itemToEdit as ICourse).description || '');
            } else {
                setDescription('');
            }

            if (type === 'lesson') {
                const lesson = itemToEdit as ILesson;
                setContentType(lesson.content.type || 'text');
                if (lesson.content.type === 'text') {
                    setContent((lesson.content as ITextLessonContent).content);
                } else if (lesson.content.type === 'video') {
                    setContent((lesson.content as IVideoLessonContent).url);
                } else if (lesson.content.type === 'quiz') {
                    setQuizQuestions((lesson.content as IQuizLessonContent).questions);
                }
            }
        } else {
            setTitle('');
            setDescription('');
            setContent('');
            setContentType('text');
            setQuizQuestions([]);
        }

        setDialogOpen(true);
    };

    const closeDialog = () => {
        setDialogOpen(false);
        setCurrentDialog(null);
        setCurrentContext(null);
        setEditItem(null);
        setTitle('');
        setDescription('');
        setContent('');
        setQuizQuestions([]);
    };

    const handleAddCourse = async () => {
        try {
            const newCourse = await createCourse({ title, description });
            onUpdateCourse([...courses, newCourse]);
            closeDialog();
        } catch (err) {
            console.error('Error creating course:', err);
        }
    };

    const handleUpdateCourse = async () => {
        if (!editItem) return;

        try {
            const updatedCourse = await updateCourse(editItem.id, { title, description });
            onUpdateCourse(courses.map(c => c.id === updatedCourse.id ? updatedCourse : c));
            closeDialog();
        } catch (err) {
            console.error('Error updating course:', err);
        }
    };

    const handleDeleteCourse = async (courseId: string) => {
        try {
            await deleteCourse(courseId);
            onUpdateCourse(courses.filter(c => c.id !== courseId));
        } catch (err) {
            console.error('Error deleting course:', err);
        }
    };

    const handleAddModule = async () => {
        if (!currentContext?.courseId) return;

        try {
            const newModule = await createModule(currentContext.courseId, { title });
            onUpdateCourse(courses.map(c =>
                c.id === currentContext.courseId
                    ? { ...c, modules: [...c.modules, newModule] }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error creating module:', err);
        }
    };

    const handleUpdateModule = async () => {
        if (!editItem || !currentContext?.courseId) return;

        try {
            const updatedModule = await updateModule(
                currentContext.courseId,
                editItem.id,
                { title }
            );

            onUpdateCourse(courses.map(c =>
                c.id === currentContext.courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === updatedModule.id ? updatedModule : m
                        )
                    }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error updating module:', err);
        }
    };

    const handleDeleteModule = async (courseId: string, moduleId: string) => {
        try {
            await deleteModule(courseId, moduleId);
            onUpdateCourse(courses.map(c =>
                c.id === courseId
                    ? {
                        ...c,
                        modules: c.modules.filter(m => m.id !== moduleId)
                    }
                    : c
            ));
        } catch (err) {
            console.error('Error deleting module:', err);
        }
    };

    const handleAddLesson = async () => {
        if (!currentContext?.courseId || !currentContext?.moduleId) return;

        try {
            let lessonContent: ILessonContent;

            switch (contentType) {
                case 'text':
                    lessonContent = {
                        type: 'text',
                        content: content
                    } as ITextLessonContent;
                    break;
                case 'video':
                    lessonContent = {
                        type: 'video',
                        url: content,
                        duration: 0
                    } as IVideoLessonContent;
                    break;
                case 'quiz':
                    lessonContent = {
                        type: 'quiz',
                        questions: quizQuestions
                    } as IQuizLessonContent;
                    break;
                default:
                    lessonContent = {
                        type: 'text',
                        content: ''
                    } as ITextLessonContent;
            }

            const newLesson = await createLesson(
                currentContext.courseId,
                currentContext.moduleId,
                {
                    title,
                    content: lessonContent
                }
            );

            onUpdateCourse(
                courses.map(c =>
                    c.id === currentContext.courseId
                        ? {
                            ...c,
                            modules: c.modules.map(m =>
                                m.id === currentContext.moduleId
                                    ? { ...m, lessons: [...m.lessons, newLesson] }
                                    : m
                            )
                        }
                        : c
                )
            );
            closeDialog();
        } catch (err) {
            console.error('Error creating lesson:', err);
        }
    };

    const handleUpdateLesson = async () => {
        if (!editItem || !currentContext?.courseId || !currentContext?.moduleId) return;

        try {
            let lessonContent: ILessonContent;

            switch (contentType) {
                case 'text':
                    lessonContent = { type: 'text', content };
                    break;
                case 'video':
                    lessonContent = { type: 'video', url: content, duration: 0 };
                    break;
                case 'quiz':
                    lessonContent = (editItem as ILesson).content.type === 'quiz'
                        ? { type: 'quiz', questions: quizQuestions }
                        : { type: 'quiz', questions: [] };
                    break;
                default:
                    lessonContent = { type: 'text', content: '' };
            }

            const updatedLesson = await updateLesson(
                currentContext.courseId,
                currentContext.moduleId,
                editItem.id,
                { title, content: lessonContent }
            );

            onUpdateCourse(courses.map(c =>
                c.id === currentContext.courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === currentContext.moduleId
                                ? {
                                    ...m,
                                    lessons: m.lessons.map(l =>
                                        l.id === updatedLesson.id ? updatedLesson : l
                                    )
                                }
                                : m
                        )
                    }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error updating lesson:', err);
        }
    };

    const handleDeleteLesson = async (courseId: string, moduleId: string, lessonId: string) => {
        try {
            await deleteLesson(courseId, moduleId, lessonId);
            onUpdateCourse(courses.map(c =>
                c.id === courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === moduleId
                                ? {
                                    ...m,
                                    lessons: m.lessons.filter(l => l.id !== lessonId)
                                }
                                : m
                        )
                    }
                    : c
            ));
        } catch (err) {
            console.error('Error deleting lesson:', err);
        }
    };

    const renderContentField = () => {
        switch (contentType) {
            case 'text':
                return (
                    <TextField
                        label="Текст урока"
                        multiline
                        rows={4}
                        fullWidth
                        value={content}
                        onChange={(e) => setContent(e.target.value)}
                        margin="normal"
                    />
                );
            case 'video':
                return (
                    <TextField
                        label="URL видео"
                        fullWidth
                        value={content}
                        onChange={(e) => setContent(e.target.value)}
                        margin="normal"
                    />
                );
            case 'quiz':
                return (
                    <Box sx={{ mt: 2 }}>
                        {quizQuestions.map((q, index) => (
                            <Box key={q.id} sx={{ mb: 3, p: 2, border: '1px solid #eee', borderRadius: 1 }}>
                                <Typography variant="subtitle1">Вопрос {index + 1}</Typography>
                                <TextField
                                    label="Текст вопроса"
                                    fullWidth
                                    value={q.question}
                                    onChange={(e) => {
                                        const updated = [...quizQuestions];
                                        updated[index].question = e.target.value;
                                        setQuizQuestions(updated);
                                    }}
                                    sx={{ mb: 2 }}
                                />

                                {q.options.map((option, optIndex) => (
                                    <Box key={option.id} sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                                        <TextField
                                            label={`Вариант ${optIndex + 1}`}
                                            fullWidth
                                            value={option.text}
                                            onChange={(e) => {
                                                const updated = [...quizQuestions];
                                                updated[index].options[optIndex].text = e.target.value;
                                                setQuizQuestions(updated);
                                            }}
                                        />
                                        <Checkbox
                                            checked={option.isCorrect}
                                            onChange={() => {
                                                const updated = [...quizQuestions];
                                                updated[index].options[optIndex].isCorrect = !option.isCorrect;
                                                setQuizQuestions(updated);
                                            }}
                                            sx={{ ml: 1 }}
                                        />
                                        <IconButton
                                            onClick={() => {
                                                const updated = [...quizQuestions];
                                                updated[index].options = updated[index].options.filter(
                                                    (_, i) => i !== optIndex
                                                );
                                                setQuizQuestions(updated);
                                            }}
                                            disabled={q.options.length <= 2}
                                        >
                                            <Delete fontSize="small" />
                                        </IconButton>
                                    </Box>
                                ))}

                                <Button
                                    startIcon={<Add />}
                                    onClick={() => {
                                        const updated = [...quizQuestions];
                                        updated[index].options.push({
                                            id: `opt-${Date.now()}`,
                                            text: '',
                                            isCorrect: false
                                        });
                                        setQuizQuestions(updated);
                                    }}
                                    sx={{ mb: 2 }}
                                >
                                    Добавить вариант
                                </Button>
                            </Box>
                        ))}

                        <Button
                            variant="outlined"
                            startIcon={<Add />}
                            onClick={() => {
                                setQuizQuestions([
                                    ...quizQuestions,
                                    {
                                        id: `q-${Date.now()}`,
                                        question: '',
                                        options: [
                                            { id: `opt-${Date.now()}-1`, text: '', isCorrect: false },
                                            { id: `opt-${Date.now()}-2`, text: '', isCorrect: false }
                                        ]
                                    }
                                ]);
                            }}
                        >
                            Добавить вопрос
                        </Button>
                    </Box>
                );
            default:
                return null;
        }
    };

    const renderLessonIcon = (contentType: 'text' | 'video' | 'quiz') => {
        switch (contentType) {
            case 'text':
                return <Article fontSize="small" />;
            case 'video':
                return <VideoLibrary fontSize="small" />;
            case 'quiz':
                return <Quiz fontSize="small" />;
            default:
                return null;
        }
    };

    if (loading && courses.length === 0) {
        return (
            <Box
                sx={{
                    display: 'flex',
                    justifyContent: 'center',
                    alignItems: 'center',
                    height: '100vh'
                }}
            >
                <MySpinner />
            </Box>
        );
    }

    if (error) {
        return (
            <Alert severity="error">
                {error}
            </Alert>
        );
    }

    return (
        <Box sx={{ width: '100%', p: 2 }}>
            <Button
                variant="contained"
                startIcon={<Add />}
                onClick={() => openDialog('course')}
                sx={{ mb: 2 }}
            >
                Добавить курс
            </Button>

            <List>
                {courses.map((course) => (
                    <React.Fragment key={course.id}>
                        <ListItem
                            secondaryAction={
                                <Box>
                                    <Tooltip title="Добавить модуль">
                                        <IconButton
                                            edge="end"
                                            onClick={() => openDialog('module', { courseId: course.id })}
                                        >
                                            <Add />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Редактировать курс">
                                        <IconButton
                                            edge="end"
                                            onClick={() => openDialog('course', undefined, course)}
                                        >
                                            <Edit />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Удалить курс">
                                        <IconButton
                                            edge="end"
                                            onClick={(e) => {
                                                e.stopPropagation();
                                                handleDeleteClick('course', { courseId: course.id });
                                            }}
                                        >
                                            <Delete fontSize="small" />
                                        </IconButton>
                                    </Tooltip>
                                </Box>
                            }
                            sx={{ bgcolor: '#f5f5f5', mt: 1, borderRadius: 1 }}
                        >
                            <IconButton onClick={() => toggleCourse(course.id)}>
                                {expandedCourses[course.id] ? <ExpandLess /> : <ExpandMore />}
                            </IconButton>
                            <ListItemText
                                primary={course.title}
                                secondary={course.description}
                            />
                        </ListItem>

                        <Collapse in={expandedCourses[course.id]} timeout="auto" unmountOnExit>
                            <List component="div" disablePadding>
                                {course.modules.map((module) => (
                                    <React.Fragment key={module.id}>
                                        <ListItem
                                            secondaryAction={
                                                <Box>
                                                    <Tooltip title="Добавить урок">
                                                        <IconButton
                                                            edge="end"
                                                            onClick={() => openDialog('lesson', {
                                                                courseId: course.id,
                                                                moduleId: module.id
                                                            })}
                                                        >
                                                            <Add />
                                                        </IconButton>
                                                    </Tooltip>
                                                    <Tooltip title="Редактировать модуль">
                                                        <IconButton
                                                            edge="end"
                                                            onClick={() => openDialog('module', {
                                                                courseId: course.id
                                                            }, module)}
                                                        >
                                                            <Edit />
                                                        </IconButton>
                                                    </Tooltip>
                                                    <Tooltip title="Удалить модуль">
                                                        <IconButton
                                                            edge="end"
                                                            onClick={(e) => {
                                                                e.stopPropagation();
                                                                handleDeleteClick('module', { courseId: course.id, moduleId: module.id });
                                                            }}
                                                        >
                                                            <Delete fontSize="small" />
                                                        </IconButton>
                                                    </Tooltip>
                                                </Box>
                                            }
                                            sx={{ bgcolor: '#f9f9f9', pl: 4, borderRadius: 1, mt: 1 }}
                                        >
                                            <IconButton onClick={() => toggleModule(module.id)}>
                                                {expandedModules[module.id] ? <ExpandLess /> : <ExpandMore />}
                                            </IconButton>
                                            <ListItemText primary={module.title} />
                                        </ListItem>

                                        <Collapse in={expandedModules[module.id]} timeout="auto" unmountOnExit>
                                            <List component="div" disablePadding>
                                                {module.lessons.map((lesson) => (
                                                    <ListItem
                                                        key={lesson.id}
                                                        secondaryAction={
                                                            <Box>
                                                                <Tooltip title="Редактировать урок">
                                                                    <IconButton
                                                                        edge="end"
                                                                        onClick={() => openDialog('lesson', {
                                                                            courseId: course.id,
                                                                            moduleId: module.id
                                                                        }, lesson)}
                                                                    >
                                                                        <Edit />
                                                                    </IconButton>
                                                                </Tooltip>
                                                                <Tooltip title="Удалить урок">
                                                                    <IconButton
                                                                        edge="end"
                                                                        onClick={(e) => {
                                                                            e.stopPropagation();
                                                                            handleDeleteClick('lesson', {
                                                                                courseId: course.id,
                                                                                moduleId: module.id,
                                                                                lessonId: lesson.id
                                                                            });
                                                                        }}
                                                                    >
                                                                        <Delete fontSize="small" />
                                                                    </IconButton>
                                                                </Tooltip>
                                                            </Box>
                                                        }
                                                        sx={{
                                                            pl: 8,
                                                            borderLeft: '3px solid',
                                                            borderColor: lesson.content.type === 'text'
                                                                ? '#4caf50'
                                                                : lesson.content.type === 'video'
                                                                    ? '#2196f3'
                                                                    : '#ff9800',
                                                            mt: 1,
                                                            borderRadius: 1
                                                        }}
                                                    >
                                                        <Box sx={{ mr: 1 }}>
                                                            {renderLessonIcon(lesson.content.type)}
                                                        </Box>
                                                        <ListItemText
                                                            primary={lesson.title}
                                                            secondary={
                                                                lesson.content.type === 'text'
                                                                    ? `${lesson.content.content.substring(0, 50)}...`
                                                                    : lesson.content.type === 'video'
                                                                        ? lesson.content.url
                                                                        : `${lesson.content.questions.length} вопросов`
                                                            }
                                                        />
                                                    </ListItem>
                                                ))}
                                            </List>
                                        </Collapse>
                                    </React.Fragment>
                                ))}
                            </List>
                        </Collapse>
                    </React.Fragment>
                ))}
            </List>

            {/* Диалоговое окно */}
            <Dialog open={dialogOpen} onClose={closeDialog} fullWidth maxWidth="md">
                <DialogTitle>
                    {editItem
                        ? `Редактировать ${currentDialog === 'course' ? 'курс' : currentDialog === 'module' ? 'модуль' : 'урок'}`
                        : `Добавить ${currentDialog === 'course' ? 'курс' : currentDialog === 'module' ? 'модуль' : 'урок'}`}
                </DialogTitle>
                <DialogContent>
                    <TextField
                        autoFocus
                        margin="dense"
                        label={currentDialog === 'course' ? 'Название курса' : currentDialog === 'module' ? 'Название модуля' : 'Название урока'}
                        fullWidth
                        value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        sx={{ mb: 2 }}
                    />

                    {currentDialog === 'course' && (
                        <TextField
                            margin="dense"
                            label="Описание курса"
                            fullWidth
                            multiline
                            rows={3}
                            value={description}
                            onChange={(e) => setDescription(e.target.value)}
                        />
                    )}

                    {currentDialog === 'lesson' && (
                        <>
                            <Box sx={{ mt: 2, mb: 2 }}>
                                <Typography variant="subtitle2">Тип контента:</Typography>
                                <Box sx={{ display: 'flex', gap: 1, mt: 1 }}>
                                    <Chip
                                        label="Текст"
                                        onClick={() => setContentType('text')}
                                        color={contentType === 'text' ? 'primary' : 'default'}
                                    />
                                    <Chip
                                        label="Видео"
                                        onClick={() => setContentType('video')}
                                        color={contentType === 'video' ? 'primary' : 'default'}
                                    />
                                    <Chip
                                        label="Тест"
                                        onClick={() => setContentType('quiz')}
                                        color={contentType === 'quiz' ? 'primary' : 'default'}
                                    />
                                </Box>
                            </Box>

                            {renderContentField()}
                        </>
                    )}
                </DialogContent>
                <DialogActions>
                    <Button onClick={closeDialog}>Отмена</Button>
                    <Button
                        onClick={() => {
                            if (currentDialog === 'course') {
                                editItem ? handleUpdateCourse() : handleAddCourse();
                            } else if (currentDialog === 'module') {
                                editItem ? handleUpdateModule() : handleAddModule();
                            } else if (currentDialog === 'lesson') {
                                editItem ? handleUpdateLesson() : handleAddLesson();
                            }
                        }}
                        disabled={
                            !title.trim() ||
                            (currentDialog === 'lesson' && contentType === 'text' && !content.trim()) ||
                            (currentDialog === 'lesson' && contentType === 'video' && !content.trim())
                        }
                        variant="contained"
                    >
                        {loading ? (
                            <CircularProgress size={24} />
                        ) : editItem ? (
                            'Сохранить'
                        ) : (
                            'Добавить'
                        )}
                    </Button>
                </DialogActions>
            </Dialog>
            <ConfirmDialog
                open={deleteDialog.open}
                title={deleteDialog.title}
                message={deleteDialog.message}
                onConfirm={handleConfirmDelete}
                onCancel={() => setDeleteDialog({ ...deleteDialog, open: false })}
                confirmText={loading ? 'Удаление...' : 'Удалить'}
            />
        </Box>
    );
};

export default CoursesList;