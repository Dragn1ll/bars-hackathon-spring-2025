import { PieChart, BarChart, LineChart } from '@mui/x-charts';
import {
    Box,
    Paper,
    Typography,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    CircularProgress,
    Alert,
    Grid,
    Chip,
    Stack,
    LinearProgress,
    AppBar,
    Toolbar
} from '@mui/material';
import React, { useState, useEffect } from 'react';
import MySpinner from "../UI/spinner/MySpinner";
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import AssignmentIcon from '@mui/icons-material/Assignment';
import ReplayIcon from '@mui/icons-material/Replay';
import SchoolIcon from '@mui/icons-material/School';
import VisibilityIcon from '@mui/icons-material/Visibility';
import TrendingUpIcon from '@mui/icons-material/TrendingUp';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';

interface GeneralStatsTabProps {
    stats: {
        total: number;
        active: number;
        excellent: number;
        good: number;
        average: number;
        weak: number;
        avgCourseDuration?: number;
        avgAssignmentTime?: number;
        avgTestRetries?: number;
        difficultModules?: {
            name: string;
            avgScore: number;
        }[];
        popularLessons?: {
            name: string;
            views: number;
        }[];
        weeklyProgress?: {
            week: number;
            completed: number;
            active: number;
        }[];
        nps?: {
            score: number;
            promoters: number;
            passives: number;
            detractors: number;
        };
    };
    filters: {
        year: string;
        courseId: string;
    };
    onFilterChange: (name: string, value: string) => void;
}

const StatCard = ({ icon, title, value, subvalue, color }: {
    icon: React.ReactNode,
    title: string,
    value: string,
    subvalue?: string,
    color?: 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning'
}) => {
    return (
        <Paper sx={{p: 2, height: '100%'}}>
            <Box display="flex" alignItems="center" mb={1}>
                {React.cloneElement(icon as React.ReactElement)}
                <Typography variant="subtitle1" ml={1}>{title}</Typography>
            </Box>
            <Typography variant="h5">{value}</Typography>
            {subvalue && <Typography variant="caption" color="text.secondary">{subvalue}</Typography>}
        </Paper>);
};

const NpsGauge = ({ score }: { score: number }) => {
    const getNpsColor = (score: number) => {
        if (score > 75) return 'success';
        if (score > 50) return 'info';
        if (score > 0) return 'warning';
        return 'error';
    };

    return (
        <Box sx={{ position: 'relative', width: '100%', mt: 2 }}>
            <LinearProgress
                variant="determinate"
                value={Math.min(Math.max(score + 100, 0), 100)}
                color={getNpsColor(score)}
                sx={{ height: 10, borderRadius: 5 }}
            />
            <Box sx={{
                position: 'absolute',
                top: -20,
                left: `${score + 50}%`,
                transform: 'translateX(-50%)',
                textAlign: 'center'
            }}>
                <Typography variant="body2" fontWeight="bold">{score}</Typography>
            </Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 1 }}>
                <Typography variant="caption">-100</Typography>
                <Typography variant="caption">0</Typography>
                <Typography variant="caption">+100</Typography>
            </Box>
        </Box>
    );
};

const GeneralStatsTab = ({ stats, filters, onFilterChange }: GeneralStatsTabProps) => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (filters.courseId !== 'all') {
            setLoading(true);
            setTimeout(() => {
                setLoading(false);
            }, 500);
        }
    }, [filters.courseId]);

    if (loading) return (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <MySpinner />
        </Box>
    );

    if (error) return (
        <Alert severity="error">{error}</Alert>
    );

    return (
        <Box sx={{ mt: 2 }}>
            <AppBar
                position="static"
                color="default"
                elevation={0}
                sx={{
                    mb: 3,
                    bgcolor: 'background.paper',
                    borderBottom: '1px solid',
                    borderColor: 'divider'
                }}
            >
                <Toolbar sx={{ justifyContent: 'space-between' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center' }}>
                        <Typography variant="h6" sx={{ mr: 2 }}>
                            Аналитика курса
                        </Typography>
                        <FormControl size="small" sx={{ minWidth: 200 }}>
                            <InputLabel>Курс</InputLabel>
                            <Select
                                value={filters.courseId}
                                onChange={(e) => onFilterChange('courseId', e.target.value)}
                                label="Курс"
                            >
                                <MenuItem value="all">Все курсы</MenuItem>
                                <MenuItem value="1">Тим Лид</MenuItem>
                            </Select>
                        </FormControl>
                    </Box>
                    <Typography variant="body2" color="text.secondary">
                        {new Date().toLocaleDateString()}
                    </Typography>
                </Toolbar>
            </AppBar>

            {/* Карточки с основными метриками */}
            <Grid container spacing={3} sx={{ mb: 3 }}>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        icon={<AccessTimeIcon />}
                        title="Среднее время на курс"
                        value={`${stats.avgCourseDuration?.toFixed(1) || '~'} ч`}
                        subvalue={`${((stats.avgCourseDuration || 0) / 24).toFixed(1)} дней`}
                        color="primary"
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        icon={<AssignmentIcon />}
                        title="Время на задание"
                        value={`${stats.avgAssignmentTime || '~'} мин`}
                        subvalue="Среднее на задание"
                        color="secondary"
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        icon={<ReplayIcon />}
                        title="Попытки тестов"
                        value={`${stats.avgTestRetries || '~'}`}
                        subvalue="Среднее количество"
                        color="info"
                    />
                </Grid>
                <Grid item xs={12} sm={6} md={3}>
                    <StatCard
                        icon={<EmojiEventsIcon />}
                        title="Оценка курса"
                        value={`${stats.nps?.score || '~'}`}
                        subvalue={`${stats.nps?.promoters || 0}%`}
                        color={stats.nps?.score && stats.nps.score > 50 ? 'success' : 'warning'}
                    />
                </Grid>
            </Grid>

            {/* Основные графики */}
            <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                    <Paper sx={{ p: 2, height: '100%' }}>
                        <Typography variant="h6" mb={2}>Распределение оценок</Typography>
                        <PieChart
                            series={[{
                                data: [
                                    { id: 0, value: stats.excellent, label: 'Отлично (>90%)' },
                                    { id: 1, value: stats.good, label: 'Хорошо (70-90%)' },
                                    { id: 2, value: stats.average, label: 'Удовлетворительно (50-70%)' },
                                    { id: 3, value: stats.weak, label: 'Неудовлетворительно (<50%)' },
                                ],
                                innerRadius: 30,
                                outerRadius: 100,
                                paddingAngle: 5,
                                cornerRadius: 5,
                            }]}
                            width={400}
                            height={300}
                        />
                    </Paper>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Paper sx={{ p: 2, height: '100%' }}>
                        <Typography variant="h6" mb={2}>Общая статистика</Typography>
                        <BarChart
                            xAxis={[{
                                scaleType: 'band',
                                data: ['Всего', 'Активные', 'Средний балл']
                            }]}
                            series={[{
                                data: [
                                    stats.total,
                                    stats.active,
                                    ((stats.excellent * 95 + stats.good * 80 + stats.average * 60 + stats.weak * 30) /
                                        (stats.excellent + stats.good + stats.average + stats.weak)).toFixed(1)
                                ],
                                color: '#1976d2'
                            }]}
                            width={400}
                            height={300}
                        />
                    </Paper>
                </Grid>
            </Grid>

            {/* Прогресс по неделям */}
            {stats.weeklyProgress && stats.weeklyProgress.length > 0 && (
                <Grid container spacing={3} sx={{ mt: 1 }}>
                    <Grid item xs={12}>
                        <Paper sx={{ p: 2 }}>
                            <Typography variant="h6" mb={2}>
                                <TrendingUpIcon color="primary" sx={{ verticalAlign: 'middle', mr: 1 }} />
                                Прогресс по неделям
                            </Typography>
                            <LineChart
                                xAxis={[{
                                    data: stats.weeklyProgress.map(w => `Неделя ${w.week}`),
                                    scaleType: 'band',
                                    label: 'Неделя'
                                }]}
                                series={[
                                    {
                                        data: stats.weeklyProgress.map(w => w.completed),
                                        label: 'Завершили',
                                        color: '#4caf50'
                                    },
                                    {
                                        data: stats.weeklyProgress.map(w => w.active),
                                        label: 'Активные',
                                        color: '#2196f3'
                                    }
                                ]}
                                width={800}
                                height={400}
                            />
                        </Paper>
                    </Grid>
                </Grid>
            )}

            {/* Отзывы */}
            {stats.nps && (
                <Grid container spacing={3} sx={{ mt: 1 }}>
                    <Grid item xs={12} md={6}>
                        <Paper sx={{ p: 2 }}>
                            <Typography variant="h6" mb={2}>Оценка курса</Typography>
                            <NpsGauge score={stats.nps.score} />
                            <Stack spacing={2} sx={{ mt: 2 }}>
                                <Box>
                                    <Typography>Промоутеры (9-10)</Typography>
                                    <LinearProgress
                                        variant="determinate"
                                        value={stats.nps.promoters}
                                        color="success"
                                        sx={{ height: 8 }}
                                    />
                                    <Typography variant="body2" textAlign="right">
                                        {stats.nps.promoters}%
                                    </Typography>
                                </Box>
                                <Box>
                                    <Typography>Пассивы (7-8)</Typography>
                                    <LinearProgress
                                        variant="determinate"
                                        value={stats.nps.passives}
                                        color="warning"
                                        sx={{ height: 8 }}
                                    />
                                    <Typography variant="body2" textAlign="right">
                                        {stats.nps.passives}%
                                    </Typography>
                                </Box>
                                <Box>
                                    <Typography>Детракторы (0-6)</Typography>
                                    <LinearProgress
                                        variant="determinate"
                                        value={stats.nps.detractors}
                                        color="error"
                                        sx={{ height: 8 }}
                                    />
                                    <Typography variant="body2" textAlign="right">
                                        {stats.nps.detractors}%
                                    </Typography>
                                </Box>
                            </Stack>
                        </Paper>
                    </Grid>
                </Grid>
            )}

            {/* Сложные модули и популярные уроки */}
            <Grid container spacing={3} sx={{ mt: 2 }}>
                <Grid item xs={12} md={6}>
                    <Paper sx={{ p: 2 }}>
                        <Typography variant="h6" mb={2}>
                            <SchoolIcon color="primary" sx={{ verticalAlign: 'middle', mr: 1 }} />
                            Сложные модули
                        </Typography>
                        {stats.difficultModules?.length ? (
                            <Box>
                                {stats.difficultModules.map((module, index) => (
                                    <Box key={index} sx={{ mb: 1, display: 'flex', justifyContent: 'space-between' }}>
                                        <Typography>{module.name}</Typography>
                                        <Chip
                                            label={`${module.avgScore.toFixed(1)}%`}
                                            color={module.avgScore < 50 ? 'error' : module.avgScore < 70 ? 'warning' : 'primary'}
                                        />
                                    </Box>
                                ))}
                            </Box>
                        ) : (
                            <Typography color="text.secondary">Нет данных</Typography>
                        )}
                    </Paper>
                </Grid>
                <Grid item xs={12} md={6}>
                    <Paper sx={{ p: 2 }}>
                        <Typography variant="h6" mb={2}>
                            <VisibilityIcon color="primary" sx={{ verticalAlign: 'middle', mr: 1 }} />
                            Популярные уроки
                        </Typography>
                        {stats.popularLessons?.length ? (
                            <Box>
                                {stats.popularLessons.map((lesson, index) => (
                                    <Box key={index} sx={{ mb: 1, display: 'flex', justifyContent: 'space-between' }}>
                                        <Typography>{lesson.name}</Typography>
                                        <Chip
                                            label={`${lesson.views} просмотров`}
                                            color="default"
                                        />
                                    </Box>
                                ))}
                            </Box>
                        ) : (
                            <Typography color="text.secondary">Нет данных</Typography>
                        )}
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
};

export default GeneralStatsTab;