import React, { useState } from 'react';
import { Box, Typography } from '@mui/material';
import CoursesList from './CoursesList';
import CourseCreateDialog from './CourseCreateDialog';
import { ICourse } from '../../types/types';

const CoursesManager: React.FC = () => {
    const [courses, setCourses] = useState<ICourse[]>([]);
    const [openCourseDialog, setOpenCourseDialog] = useState(false);

    const handleAddCourse = (title: string, description?: string) => {
        const newCourse: ICourse = {
            id: `course-${Date.now()}`,
            title,
            description,
            modules: []
        };
        setCourses([...courses, newCourse]);
    };

    const updateCourse = (updatedCourses: ICourse[]) => {
        setCourses(updatedCourses);
    };

    return (
        <Box>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 2 }}>
                <Typography variant="h4">Управление курсами</Typography>
            </Box>

            <CoursesList
                courses={courses}
                onUpdateCourse={updateCourse}
            />

            <CourseCreateDialog
                open={openCourseDialog}
                onClose={() => setOpenCourseDialog(false)}
                onAdd={handleAddCourse}
            />
        </Box>
    );
};

export default CoursesManager;