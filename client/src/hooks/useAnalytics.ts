import { useState, useEffect } from 'react';
import apiClient from '../API/ClientService';
import { IUser } from '../types/types';

interface AnalyticsData {
    users: IUser[];
    overallStats: {
        total: number;
        active: number;
        excellent: number;
        good: number;
        average: number;
        weak: number;
    };
    monthlyActivity: {
        month: string;
        activeUsers: number;
    }[];
    courseStats?: {
        courseId: string;
        totalAttempts: number;
        averageScore: number;
    };
    courses: {
        id: string;
        name: string;
    }[];
}

interface TestResultData {
    userId: number;
    courseId: string;
    moduleId: string;
    lessonId: string;
    score: number;
}

const useAnalytics = () => {
    const [data, setData] = useState<AnalyticsData | null>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const fetchOverallStats = async () => {
        try {
            const response = await apiClient.get('/analytics/overall');
            return response.data;
        } catch (err) {
            throw new Error('Ошибка загрузки данных!');
        }
    };

    const fetchIndividualStats = async () => {
        try {
            const response = await apiClient.get('/analytics/individual');
            return response.data;
        } catch (err) {
            throw new Error('Не удалось загрузить индивидуальную статистику');
        }
    };

    const fetchActivityData = async (year: string) => {
        try {
            const response = await apiClient.get(`/analytics/activity?year=${year}`);
            return response.data;
        } catch (err) {
            throw new Error('Не удалось загрузить данные активности');
        }
    };

    const fetchCourseStats = async (courseId: string) => {
        try {
            const response = await apiClient.get(`/analytics/course/${courseId}`);
            return response.data;
        } catch (err) {
            throw new Error('Не удалось загрузить статистику по курсу');
        }
    };

    const saveTestResult = async (resultData: TestResultData) => {
        try {
            const response = await apiClient.post('/test-results', resultData);
            return response.data;
        } catch (err) {
            throw new Error('Не удалось сохранить результат теста');
        }
    };
    const fetchCourses = async () => {
        try {
            const response = await apiClient.get('/courses');
            return response.data;
        } catch (err) {
            throw new Error('Не удалось загрузить список курсов');
        }
    };

    const loadAnalyticsData = async (year: string, courseId: string) => {
        setLoading(true);
        setError(null);

        try {
            const [overallStats, individualStats, activityData, courses] = await Promise.all([
                fetchOverallStats(),
                fetchIndividualStats(),
                fetchActivityData(year),
                fetchCourses(),
            ]);

            let courseStats = null;
            if (courseId !== 'all') {
                courseStats = await fetchCourseStats(courseId);
            }

            setData({
                users: individualStats,
                overallStats: {
                    ...overallStats,
                    ...(courseStats || {}),
                },
                monthlyActivity: activityData,
                courses,
                ...(courseStats ? { courseStats } : {}),
            });
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Неизвестная ошибка');
        } finally {
            setLoading(false);
        }
    };
    // const loadAnalyticsData = async (year: string, courseId: string) => {
    //     setLoading(true);
    //     setError(null);
    //
    //     try {
    //         const [overallStats, individualStats, activityData] = await Promise.all([
    //             fetchOverallStats(),
    //             fetchIndividualStats(),
    //             fetchActivityData(year),
    //         ]);
    //
    //         let courseStats = null;
    //         if (courseId !== 'all') {
    //             courseStats = await fetchCourseStats(courseId);
    //         }
    //
    //         setData({
    //             users: individualStats,
    //             overallStats: {
    //                 ...overallStats,
    //                 ...(courseStats || {}),
    //             },
    //             monthlyActivity: activityData,
    //             ...(courseStats ? { courseStats } : {}),
    //         });
    //     } catch (err) {
    //         setError(err instanceof Error ? err.message : 'Неизвестная ошибка');
    //     } finally {
    //         setLoading(false);
    //     }
    // };

    const refreshData = () => {
        if (data) {
            const year = '2025';
            const courseId = 'all';
            loadAnalyticsData(year, courseId);
        }
    };

    return {
        data,
        loading,
        error,
        loadAnalyticsData,
        saveTestResult,
        refreshData,
    };
};

export default useAnalytics;