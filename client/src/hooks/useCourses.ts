import { useState } from 'react';
import apiClient from '../API/ClientService';
import { IAxiosApiError, ICourse, ILesson, IModule } from "../types/types";

const
    useCourses = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const fetchCourses = async () => {
        try {
            setLoading(true);
            const response = await apiClient.get<ICourse[]>('/courses');
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to fetch courses');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const createCourse = async (courseData: Omit<ICourse, 'id' | 'modules'>) => {
        try {
            setLoading(true);
            const response = await apiClient.post<ICourse>('/courses', courseData);
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to create course');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const updateCourse = async (courseId: string, updates: Partial<ICourse>) => {
        try {
            setLoading(true);
            const response = await apiClient.patch<ICourse>(
                `/courses/${courseId}`,
                updates
            );
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to update course');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const deleteCourse = async (courseId: string) => {
        try {
            setLoading(true);
            await apiClient.delete(`/courses/${courseId}`);
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to delete course');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const createModule = async (courseId: string, moduleData: Omit<IModule, 'id' | 'lessons'>) => {
        try {
            setLoading(true);
            const response = await apiClient.post<IModule>(
                `/courses/${courseId}/modules`,
                moduleData
            );
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to create module');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const updateModule = async (
        courseId: string,
        moduleId: string,
        updates: Partial<IModule>
    ) => {
        try {
            setLoading(true);
            const response = await apiClient.patch<IModule>(
                `/courses/${courseId}/modules/${moduleId}`,
                updates
            );
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to update module');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const deleteModule = async (courseId: string, moduleId: string) => {
        try {
            setLoading(true);
            await apiClient.delete(`/courses/${courseId}/modules/${moduleId}`);
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to delete module');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const createLesson = async (
        courseId: string,
        moduleId: string,
        lessonData: Omit<ILesson, 'id'>
    ) => {
        try {
            setLoading(true);
            const response = await apiClient.post<ILesson>(
                `/courses/${courseId}/modules/${moduleId}/lessons`,
                lessonData
            );
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to create lesson');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const updateLesson = async (
        courseId: string,
        moduleId: string,
        lessonId: string,
        updates: Partial<ILesson>
    ) => {
        try {
            setLoading(true);
            const response = await apiClient.patch<ILesson>(
                `/courses/${courseId}/modules/${moduleId}/lessons/${lessonId}`,
                updates
            );
            return response.data;
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to update lesson');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    const deleteLesson = async (courseId: string, moduleId: string, lessonId: string) => {
        try {
            setLoading(true);
            await apiClient.delete(
                `/courses/${courseId}/modules/${moduleId}/lessons/${lessonId}`
            );
        } catch (err: unknown) {
            const error = err as IAxiosApiError;
            setError(error.response?.data?.message || 'Failed to delete lesson');
            throw error;
        } finally {
            setLoading(false);
        }
    };

    return {
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
        resetError: () => setError(null),
    };
};

export default useCourses;