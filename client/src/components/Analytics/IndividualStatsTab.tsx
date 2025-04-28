import { DataGrid, GridColDef } from '@mui/x-data-grid';
import dayjs from 'dayjs';
import {
    Box,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    CircularProgress,
    Alert,
    Typography,
    Avatar,
    Chip,
    LinearProgress
} from '@mui/material';
import { IUser } from "../../types/types";
import React, { useState, useEffect } from 'react';
import MySpinner from "../UI/spinner/MySpinner";
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import GradeIcon from '@mui/icons-material/Grade';
import {DataGridPro} from "@mui/x-data-grid-pro";

interface IndividualStatsTabProps {
    users: IUser[];
    filters: {
        year: string;
        courseId: string;
    };
    onFilterChange: (name: string, value: string) => void;
}

const getInitials = (name: string) => {
    if (!name) return '';
    return name.split(' ').map(n => n[0]).join('').toUpperCase();
};

const getScoreColor = (score: number) => {
    if (score >= 90) return 'success';
    if (score >= 70) return 'info';
    if (score >= 50) return 'warning';
    return 'error';
};

const IndividualStatsTab = ({ users = [], filters, onFilterChange }: IndividualStatsTabProps) => {
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [processedUsers, setProcessedUsers] = useState<any[]>([]);

    useEffect(() => {
        try {
            setLoading(true);

            const processed = users
                .map(user => {
                    const courses = Array.isArray(user.courses) ? user.courses : [];
                    const currentCourse = filters.courseId !== 'all'
                        ? courses.find(c => c?.id === filters.courseId)
                        : null;

                    return {
                        id: user.id || '',
                        fullName: `${user.name || ''} ${user.surname || ''}`.trim() || 'Не указано',
                        email: user.email || 'Не указан',
                        lastActive: user.lastActive ? dayjs(user.lastActive).format('DD.MM.YYYY HH:mm') : 'Нет данных',
                        avgScore: currentCourse?.score ||
                            (courses.reduce((sum, c) => sum + (c?.score || 0), 0) / (courses.length || 1)),
                        progress: currentCourse?.progress ||
                            (courses.filter(c => c?.completed).length / (courses.length || 1) * 100),
                        completedCourses: courses.filter(c => c?.completed).length,
                        learningHours: user.learningHours || 0,
                        testAttempts: currentCourse?.testAttempts ||
                            courses.reduce((sum, c) => sum + (c?.testAttempts || 0), 0),
                        lastCourseAccess: currentCourse?.lastAccess ||
                            (courses.length ? dayjs(Math.max(...courses.map(c =>
                                new Date(c?.lastAccess || 0).getTime()))).format('DD.MM.YYYY') : 'Нет данных')
                    };
                });

            setProcessedUsers(processed);
            setError(null);
        } catch (err) {
            console.error('Error processing user data:', err);
            setError('Произошла ошибка при обработке данных');
            setProcessedUsers([]);
        } finally {
            setLoading(false);
        }
    }, [users, filters.courseId]);

    const columns: GridColDef[] = [
        {
            field: 'fullName',
            headerName: 'Пользователь',
            width: 200,
            renderCell: (params) => (
                <Box display="flex" alignItems="center">
                    <Avatar src={params.row.avatar} sx={{ mr: 2 }}>
                        {getInitials(params.row.fullName)}
                    </Avatar>
                    {params.row.fullName}
                </Box>
            )
        },
        {
            field: 'username',
            headerName: 'Имя пользователя',
            width: 150,
        },
        {
            field: 'email',
            headerName: 'Эл. почта',
            width: 200
        },
        {
            field: 'avgScore',
            headerName: 'Средний балл',
            width: 150,
            type: 'number',
            renderCell: (params) => (
                <Box width="100%">
                    <LinearProgress
                        variant="determinate"
                        value={params.value}
                        color={getScoreColor(params.value)}
                        sx={{ height: 6, mb: 1 }}
                    />
                    <Typography variant="body2">{Math.round(params.value)}%</Typography>
                </Box>
            )
        },
        {
            field: 'progress',
            headerName: 'Прогресс',
            width: 150,
            type: 'number',
            renderCell: (params) => (
                <Box display="flex" alignItems="center">
                    <CheckCircleIcon color="primary" sx={{ mr: 1 }} />
                    <Typography>{Math.round(params.value)}%</Typography>
                </Box>
            )
        },
        {
            field: 'completedCourses',
            headerName: 'Завершено курсов',
            width: 150,
            type: 'number'
        },
        {
            field: 'learningHours',
            headerName: 'Часов обучения',
            width: 150,
            type: 'number',
            renderCell: (params) => (
                <Box display="flex" alignItems="center">
                    <AccessTimeIcon color="action" sx={{ mr: 1 }} />
                    <Typography>{params.value}</Typography>
                </Box>
            )
        },
        {
            field: 'testAttempts',
            headerName: 'Попыток тестов',
            width: 150,
            type: 'number'
        },
        {
            field: 'lastActive',
            headerName: 'Последняя активность',
            width: 180
        },

    ];

    if (loading) return (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <MySpinner />
        </Box>
    );

    if (error) return (
        <Alert severity="error">{error}</Alert>
    );

    if (!processedUsers.length) {
        return (
            <Box sx={{ mt: 2, p: 2 }}>
                <Typography variant="h6">Нет данных для отображения</Typography>
                <Typography variant="body1">
                    Не найдено ни одного студента с доступными данными.
                </Typography>
            </Box>
        );
    }

    return (
        <Box sx={{ height: 'calc(100vh - 200px)', width: '100%', mt: 2 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                <FormControl size="small" sx={{ minWidth: 200 }}>
                    <InputLabel>Курс</InputLabel>
                    <Select
                        value={filters.courseId}
                        onChange={(e) => onFilterChange('courseId', e.target.value)}
                        label="Курс"
                    >
                        <MenuItem value="all">Все курсы</MenuItem>
                        <MenuItem value="1">Тим лид</MenuItem>
                    </Select>
                </FormControl>
            </Box>

            <DataGrid
                rows={processedUsers}
                columns={columns}
                getRowId={(row) => row.id}
                pageSizeOptions={[10, 25, 50]}
                initialState={{
                    pagination: {
                        paginationModel: { page: 0, pageSize: 10 },
                    },
                }}
                sx={{
                    '& .MuiDataGrid-cell': {
                        display: 'flex',
                        alignItems: 'center'
                    }
                }}
                localeText={{
                    noRowsLabel: 'Нет данных для отображения',
                    footerRowSelected: (count) =>
                        count !== 1
                            ? `${count.toLocaleString()} студентов выбрано`
                            : '1 пользователь выбран',
                }}
            />
        </Box>
    );
};

export default IndividualStatsTab;