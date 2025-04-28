import React, { useState, useEffect } from 'react';
import { Tabs, Tab, Box, CircularProgress, Alert } from '@mui/material';
import useAnalytics from "../../hooks/useAnalytics";
import MySpinner from "../UI/spinner/MySpinner";
import ActivityTab from "./ActivityTab";
import GeneralStatsTab from "./GeneralStatsTab";
import IndividualStatsTab from "./IndividualStatsTab";

const Analytics = () => {
    const [tabValue, setTabValue] = useState(0);
    const [filters, setFilters] = useState({
        year: '2025',
        courseId: 'all',
    });

    const { data, loading, error, loadAnalyticsData } = useAnalytics();

    useEffect(() => {
        loadAnalyticsData(filters.year, filters.courseId);
    }, [filters.year, filters.courseId]);

    const handleFilterChange = (name: string, value: string) => {
        setFilters(prev => ({ ...prev, [name]: value }));
    };

    if (loading) return (
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
            <MySpinner />
        </Box>
    );

    if (error) return (
        <Alert severity="error">{error}</Alert>
    );

    if (!data) return <Alert severity="info">Нет данных для отображения</Alert>;

    return (
        <Box sx={{ p: 1 }}>
            <Tabs value={tabValue} onChange={(_, newValue) => setTabValue(newValue)}>
                <Tab label="Общая статистика" />
                <Tab label="Индивидуальная" />
                <Tab label="Динамика активности" />
            </Tabs>

            {tabValue === 0 && (
                <GeneralStatsTab
                    stats={data.overallStats}
                    filters={filters}
                    onFilterChange={handleFilterChange}
                />
            )}

            {tabValue === 1 && (
                <IndividualStatsTab
                    users={data.users}
                    filters={filters}
                    onFilterChange={handleFilterChange}
                />
            )}

            {tabValue === 2 && (
                <ActivityTab
                    activity={data.monthlyActivity}
                    filters={filters}
                    onFilterChange={handleFilterChange}
                    courses={data.courses}
                />
            )}
        </Box>
    );
};

export default Analytics;