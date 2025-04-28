import React, { useState } from 'react';
import { Box, Tabs, Tab, CircularProgress, Alert, Typography } from '@mui/material';
import useProgress from '../../hooks/useProgress';
import ProgressSummary from './ProgressSummary';
import UserProgressDetails from './UserProgressDetails';
import UsersList from "./UsersList";
import MySpinner from "../UI/spinner/MySpinner";

const ProgressReport = () => {
    const [tabValue, setTabValue] = useState(0);
    const [selectedUserId, setSelectedUserId] = useState<string | null>(null);
    const { data, loading, error, loadData } = useProgress();

    React.useEffect(() => {
        loadData(selectedUserId || undefined);
    }, [selectedUserId]);

    const handleUserSelect = (userId: string) => {
        setSelectedUserId(userId);
        setTabValue(1);
    };

    if (loading) return (
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
    if (error) return (
        <Alert severity="error">
            {error}
        </Alert>
    )
    if (!data) return <Alert>Нет данных!</Alert>;

    return (
        <Box sx={{ p: 2 }}>
            <Tabs value={tabValue} onChange={(_, newValue) => setTabValue(newValue)}>
                <Tab label="Общая статистика" />
                <Tab label="Список пользователей" />
                <Tab label="Детализация по пользователю" disabled={!selectedUserId} />
            </Tabs>

            <Box sx={{ mt: 2 }}>
                {tabValue === 0 && (
                    <ProgressSummary
                        courses={data.courses}
                        progressData={data.userProgress}
                    />
                )}

                {tabValue === 1 && (
                    <UsersList
                        users={data.userProgress.map(p => p.user)}
                    />
                )}

                {/*{tabValue === 2 && selectedUserId && data.selectedUserProgress && (*/}
                {/*    <UserProgressDetails*/}
                {/*        progress={data.selectedUserProgress}*/}
                {/*        courses={data.courses}*/}
                {/*    />*/}
                {/*)}*/}

            </Box>
        </Box>
    );
};

export default ProgressReport;