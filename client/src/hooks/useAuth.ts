import {useCallback, useEffect, useState} from 'react';
import { useNavigate } from 'react-router-dom';
import { useLazyQuery, useMutation, gql } from '@apollo/client';
import { IAuthUser } from "../types/types";
import {CHECK_AUTH_QUERY, LOGIN_MUTATION, LOGOUT_MUTATION} from "../query/user";

export const useAuth = () => {
    const [user, setUser] = useState<IAuthUser | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const navigate = useNavigate();
    const [loginMutation] = useMutation(LOGIN_MUTATION);
    const [checkAuthQuery] = useLazyQuery(CHECK_AUTH_QUERY);

    const login = useCallback(async (email: string, password: string) => {
        try {
            setIsLoading(true);
            const { data } = await loginMutation({
                variables: { email, password }
            });

            if (data?.login?.token) {
                localStorage.setItem('token', data.login.token);
                setUser(data.login.user);
                return true;
            }
            return false;
        } catch (error) {
            console.error('Login error:', error);
            logout();
            return false;
        } finally {
            setIsLoading(false);
        }
    }, [loginMutation]);

    const logout = useCallback(() => {
        localStorage.removeItem('token');
        setUser(null);
        navigate('/login');
    }, [navigate]);

    const checkAuth = useCallback(async () => {
        try {
            setIsLoading(true);
            const { data } = await checkAuthQuery();
            if (data?.checkAuth) {
                setUser(data.checkAuth);
                return true;
            }
            return false;
        } catch (error) {
            console.error('Auth check error:', error);
            return false;
        } finally {
            setIsLoading(false);
        }
    }, [checkAuthQuery]);

    const isAuthenticated = useCallback(() => !!user, [user]);
    const hasRole = useCallback((role: string) => user?.roles.includes(role) ?? false, [user]);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            checkAuth();
        } else {
            setIsLoading(false);
        }
        const handleStorageChange = (e: StorageEvent) => {
            if (e.key === 'token') {
                if (e.newValue) {
                    checkAuth();
                } else {
                    logout();
                }
            }
        };

        window.addEventListener('storage', handleStorageChange);
        return () => window.removeEventListener('storage', handleStorageChange);
    }, [checkAuth]);

    return {
        user,
        isLoading,
        login,
        logout,
        isAuthenticated,
        hasRole
    };
};