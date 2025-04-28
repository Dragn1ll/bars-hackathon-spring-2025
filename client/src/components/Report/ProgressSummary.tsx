import React from 'react';
import { Box, Typography, Paper, Grid, LinearProgress } from '@mui/material';
import { ICourse, IUserProgress } from '../../types/types';

interface ProgressSummaryProps {
    courses: ICourse[];
    progressData: IUserProgress[];
}

const ProgressSummary = ({ courses, progressData }: ProgressSummaryProps) => {
    const courseStats = courses.map(course => {
        const courseProgress = progressData.filter(p => p.courseId === course.id);
        const avgProgress = courseProgress.reduce((sum, p) => sum + p.progress, 0) / courseProgress.length || 0;

        return {
            ...course,
            avgProgress,
            usersCount: courseProgress.length,
        };
    });

    return (
        <Box>
            <Typography variant="h5" gutterBottom>
                Общая статистика прохождения
            </Typography>

            <Grid container spacing={3}>
                {courseStats.map(course => (
                    <Grid item xs={12} md={6} lg={4} key={course.id}>
                        <Paper sx={{ p: 2 }}>
                            <Typography variant="h6">{course.title}</Typography>
                            <Typography color="text.secondary">
                                {course.usersCount} пользователей
                            </Typography>

                            <Box sx={{ mt: 2 }}>
                                <Typography>Средний прогресс:</Typography>
                                <LinearProgress
                                    variant="determinate"
                                    value={course.avgProgress}
                                    sx={{ height: 10, mt: 1 }}
                                />
                                <Typography textAlign="right">
                                    {Math.round(course.avgProgress)}%
                                </Typography>
                            </Box>

                            {/* Дополнительная статистика по модулям */}
                            <Box sx={{ mt: 2 }}>
                                <Typography variant="subtitle2">Прогресс по модулям:</Typography>
                                {course.modules.map(module => (
                                    <Box key={module.id} sx={{ mt: 1 }}>
                                        <Typography>{module.title}</Typography>
                                        <LinearProgress
                                            variant="determinate"
                                            value={
                                                progressData
                                                    .filter(p => p.courseId === course.id)
                                                    .reduce((sum, p) => sum + (p.modules[module.id]?.progress || 0), 0) /
                                                progressData.length || 0
                                            }
                                            sx={{ height: 8 }}
                                        />
                                    </Box>
                                ))}
                            </Box>
                        </Paper>
                    </Grid>
                ))}
            </Grid>
        </Box>
    );
};

export default ProgressSummary;