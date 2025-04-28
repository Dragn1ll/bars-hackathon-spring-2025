import { useState, useEffect, useCallback } from 'react';
import apiClient from '../API/ClientService';
import {IUser} from "../types/types"

const useUsers = () => {
    const [users, setUsers] = useState<IUser[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchUsers = useCallback(async () => {
        try {
            setLoading(true);
            const response = await apiClient.get<IUser[]>('/users');
            setUsers(response.data.filter(user => !user.isDeleted));
            setError(null);
        } catch (err) {
            setError('Ошибка загрузки данных!');
        } finally {
            setLoading(false);
        }
    }, []);

    const addUser = async (user: Omit<IUser, 'id'>) => {
        try {
            const response = await apiClient.post<IUser>('/users', user);
            setUsers(prev => [...prev, response.data]);
            return true;
        } catch (err) {
            return false;
        }
    };

    const deleteUser = async (id: number) => {
        try {
            await apiClient.delete(`/users/${id}`);
            setUsers(prev => prev.filter(user => user.id !== id));
            return true;
        } catch (err) {
            return false;
        }
    };

    const softDeleteUser = async (id: number) => {
        try {
            await apiClient.patch(`/users/${id}`, { isDeleted: true });
            setUsers(prev => prev.filter(user => user.id !== id));
            return true;
        } catch (err) {
            return false;
        }
    };

    const restoreUser = async (id: number) => {
        try {
            await apiClient.patch(`/users/${id}`, { isDeleted: false });
            await fetchUsers();
            return true;
        } catch (err) {
            return false;
        }
    };

    const getDeletedUsers = async () => {
        try {
            const response = await apiClient.get<IUser[]>('/users?is_deleted=true');
            return response.data.filter(user => user.isDeleted);
        } catch (err) {
            return [];
        }
    };

    useEffect(() => {
        fetchUsers();
    }, [fetchUsers]);

    return {
        users,
        loading,
        error,
        addUser,
        deleteUser,
        // updateUser,
        softDeleteUser,
        restoreUser,
        getDeletedUsers,
        fetchUsers,
        refreshUsers: fetchUsers,
    };
};

export default useUsers;