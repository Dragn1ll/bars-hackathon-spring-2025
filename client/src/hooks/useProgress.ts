import { useState, useEffect } from 'react';
import apiClient from '../API/ClientService';
import {IProgressData} from '../types/types';

const useProgress = () => {
    const [data, setData] = useState<IProgressData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchCourses = async () => {
        try {
            const response = await apiClient.get('/courses/structure');
            return response.data;
        } catch (err) {
            throw new Error('Ошибка загрузки данных!');
        }
    };

    const fetchAllUsersProgress = async () => {
        try {
            const response = await apiClient.get('/progress/all');
            return response.data;
        } catch (err) {
            throw new Error('Failed to fetch progress data');
        }
    };

    const fetchUserDetailedProgress = async (userId: string) => {
        try {
            const response = await apiClient.get(`/progress/user/${userId}`);
            return response.data;
        } catch (err) {
            throw new Error('Failed to fetch user progress');
        }
    };

    const loadData = async (userId?: string) => {
        setLoading(true);
        setError(null);

        try {
            const [courses, userProgress] = await Promise.all([
                fetchCourses(),
                fetchAllUsersProgress(),
            ]);

            let selectedUserProgress = null;
            if (userId) {
                selectedUserProgress = await fetchUserDetailedProgress(userId);
            }

            setData({
                courses,
                userProgress,
                selectedUserProgress,
            });
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Unknown error');
        } finally {
            setLoading(false);
        }
    };

    const refreshData = (userId?: string) => {
        loadData(userId);
    };

    return {
        data,
        loading,
        error,
        loadData,
        refreshData,
    };
};

export default useProgress;