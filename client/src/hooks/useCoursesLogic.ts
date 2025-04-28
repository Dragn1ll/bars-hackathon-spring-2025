import { useState, useCallback } from 'react';
import {
    ICourse,
    IModule,
    ILesson,
    ILessonContent,
    ITextLessonContent,
    IVideoLessonContent,
    IQuizLessonContent, IQuizQuestion
} from '../types/types';
import useCourses from './useCourses';

export const useCoursesLogic = (initialCourses: ICourse[], onUpdateCourses: (courses: ICourse[]) => void) => {
    const {
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
        resetError
    } = useCourses();

    const [expandedCourses, setExpandedCourses] = useState<Record<string, boolean>>({});
    const [expandedModules, setExpandedModules] = useState<Record<string, boolean>>({});
    const [dialogOpen, setDialogOpen] = useState(false);
    const [currentDialog, setCurrentDialog] = useState<'course' | 'module' | 'lesson' | null>(null);
    const [currentContext, setCurrentContext] = useState<{
        courseId?: string;
        moduleId?: string;
    } | null>(null);
    const [editItem, setEditItem] = useState<ICourse | IModule | ILesson | null>(null);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [contentType, setContentType] = useState<'text' | 'video' | 'quiz'>('text');
    const [content, setContent] = useState('');
    const [quizQuestions, setQuizQuestions] = useState<IQuizQuestion[]>([]);
    const [deleteDialog, setDeleteDialog] = useState({
        open: false,
        type: '',
        data: { courseId: '', moduleId: '', lessonId: '' },
        title: '',
        message: ''
    });

    const toggleCourse = useCallback((courseId: string) => {
        setExpandedCourses(prev => ({
            ...prev,
            [courseId]: !prev[courseId]
        }));
    }, []);

    const toggleModule = useCallback((moduleId: string) => {
        setExpandedModules(prev => ({
            ...prev,
            [moduleId]: !prev[moduleId]
        }));
    }, []);

    const openDialog = useCallback((
        type: 'course' | 'module' | 'lesson',
        context?: { courseId?: string; moduleId?: string },
        itemToEdit?: ICourse | IModule | ILesson
    ) => {
        setCurrentDialog(type);
        setCurrentContext(context || null);
        setEditItem(itemToEdit || null);

        if (itemToEdit) {
            setTitle('title' in itemToEdit ? itemToEdit.title : '');

            if (type === 'course') {
                setDescription((itemToEdit as ICourse).description || '');
            } else {
                setDescription('');
            }

            if (type === 'lesson') {
                const lesson = itemToEdit as ILesson;
                setContentType(lesson.content.type);
                if (lesson.content.type === 'text') {
                    setContent((lesson.content as ITextLessonContent).content);
                } else if (lesson.content.type === 'video') {
                    setContent((lesson.content as IVideoLessonContent).url);
                } else if (lesson.content.type === 'quiz') {
                    setQuizQuestions((lesson.content as IQuizLessonContent).questions);
                }
            }
        } else {
            setTitle('');
            setDescription('');
            setContent('');
            setContentType('text');
            setQuizQuestions([]);
        }

        setDialogOpen(true);
    }, []);

    const closeDialog = useCallback(() => {
        setDialogOpen(false);
        setCurrentDialog(null);
        setCurrentContext(null);
        setEditItem(null);
        setTitle('');
        setDescription('');
        setContent('');
        setQuizQuestions([]);
    }, []);

    const handleDeleteClick = useCallback((type: 'course' | 'module' | 'lesson', ids: { courseId?: string, moduleId?: string, lessonId?: string }) => {
        const messages = {
            course: 'Вы уверены, что хотите удалить этот курс? Это действие нельзя отменить.',
            module: 'Вы уверены, что хотите удалить этот модуль? Все уроки в нем будут удалены.',
            lesson: 'Вы уверены, что хотите удалить этот урок?'
        };

        setDeleteDialog({
            open: true,
            type,
            data: {
                courseId: ids.courseId || '',
                moduleId: ids.moduleId || '',
                lessonId: ids.lessonId || ''
            },
            title: `Удаление ${type === 'course' ? 'курса' : type === 'module' ? 'модуля' : 'урока'}`,
            message: messages[type]
        });
    }, []);

    const handleConfirmDelete = useCallback(async () => {
        try {
            const { courseId, moduleId, lessonId } = deleteDialog.data;

            switch (deleteDialog.type) {
                case 'course':
                    await deleteCourse(courseId);
                    break;
                case 'module':
                    await deleteModule(courseId, moduleId);
                    break;
                case 'lesson':
                    await deleteLesson(courseId, moduleId, lessonId);
                    break;
            }

            const updatedCourses = await fetchCourses();
            onUpdateCourses(updatedCourses);
        } catch (err) {
            console.error('Ошибка при удалении:', err);
        } finally {
            setDeleteDialog(prev => ({ ...prev, open: false }));
        }
    }, [deleteDialog, deleteCourse, deleteModule, deleteLesson, fetchCourses, onUpdateCourses]);

    const handleCancelDelete = useCallback(() => {
        setDeleteDialog(prev => ({ ...prev, open: false }));
    }, []);

    const handleAddCourse = useCallback(async () => {
        try {
            const newCourse = await createCourse({ title, description });
            onUpdateCourses([...initialCourses, newCourse]);
            closeDialog();
        } catch (err) {
            console.error('Error creating course:', err);
        }
    }, [title, description, initialCourses, createCourse, onUpdateCourses, closeDialog]);

    const handleUpdateCourse = useCallback(async () => {
        if (!editItem) return;

        try {
            const updatedCourse = await updateCourse(editItem.id, { title, description });
            onUpdateCourses(initialCourses.map(c => c.id === updatedCourse.id ? updatedCourse : c));
            closeDialog();
        } catch (err) {
            console.error('Error updating course:', err);
        }
    }, [editItem, title, description, initialCourses, updateCourse, onUpdateCourses, closeDialog]);

    const handleAddModule = useCallback(async () => {
        if (!currentContext?.courseId) return;

        try {
            const newModule = await createModule(currentContext.courseId, { title });
            onUpdateCourses(initialCourses.map(c =>
                c.id === currentContext.courseId
                    ? { ...c, modules: [...c.modules, newModule] }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error creating module:', err);
        }
    }, [currentContext, title, initialCourses, createModule, onUpdateCourses, closeDialog]);

    const handleUpdateModule = useCallback(async () => {
        if (!editItem || !currentContext?.courseId) return;

        try {
            const updatedModule = await updateModule(
                currentContext.courseId,
                editItem.id,
                { title }
            );

            onUpdateCourses(initialCourses.map(c =>
                c.id === currentContext.courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === updatedModule.id ? updatedModule : m
                        )
                    }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error updating module:', err);
        }
    }, [editItem, currentContext, title, initialCourses, updateModule, onUpdateCourses, closeDialog]);

    const handleAddLesson = useCallback(async () => {
        if (!currentContext?.courseId || !currentContext?.moduleId) return;

        try {
            let lessonContent: ILessonContent;

            switch (contentType) {
                case 'text':
                    lessonContent = { type: 'text', content };
                    break;
                case 'video':
                    lessonContent = { type: 'video', url: content, duration: 0 };
                    break;
                case 'quiz':
                    lessonContent = { type: 'quiz', questions: quizQuestions };
                    break;
                default:
                    lessonContent = { type: 'text', content: '' };
            }

            const newLesson = await createLesson(
                currentContext.courseId,
                currentContext.moduleId,
                { title, content: lessonContent }
            );

            onUpdateCourses(initialCourses.map(c =>
                c.id === currentContext.courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === currentContext.moduleId
                                ? { ...m, lessons: [...m.lessons, newLesson] }
                                : m
                        )
                    }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error creating lesson:', err);
        }
    }, [currentContext, title, contentType, content, quizQuestions, initialCourses, createLesson, onUpdateCourses, closeDialog]);

    const handleUpdateLesson = useCallback(async () => {
        if (!editItem || !currentContext?.courseId || !currentContext?.moduleId) return;

        try {
            let lessonContent: ILessonContent;

            switch (contentType) {
                case 'text':
                    lessonContent = { type: 'text', content };
                    break;
                case 'video':
                    lessonContent = { type: 'video', url: content, duration: 0 };
                    break;
                case 'quiz':
                    lessonContent = { type: 'quiz', questions: quizQuestions };
                    break;
                default:
                    lessonContent = { type: 'text', content: '' };
            }

            const updatedLesson = await updateLesson(
                currentContext.courseId,
                currentContext.moduleId,
                editItem.id,
                { title, content: lessonContent }
            );

            onUpdateCourses(initialCourses.map(c =>
                c.id === currentContext.courseId
                    ? {
                        ...c,
                        modules: c.modules.map(m =>
                            m.id === currentContext.moduleId
                                ? {
                                    ...m,
                                    lessons: m.lessons.map(l =>
                                        l.id === updatedLesson.id ? updatedLesson : l
                                    )
                                }
                                : m
                        )
                    }
                    : c
            ));
            closeDialog();
        } catch (err) {
            console.error('Error updating lesson:', err);
        }
    }, [editItem, currentContext, title, contentType, content, quizQuestions, initialCourses, updateLesson, onUpdateCourses, closeDialog]);

    return {
        // Состояния
        loading,
        error,
        expandedCourses,
        expandedModules,
        dialogOpen,
        currentDialog,
        currentContext,
        editItem,
        deleteDialog,
        title,
        description,
        contentType,
        content,
        quizQuestions,

        // Методы
        toggleCourse,
        toggleModule,
        openDialog,
        closeDialog,
        handleDeleteClick,
        handleConfirmDelete,
        handleCancelDelete,
        handleAddCourse,
        handleUpdateCourse,
        handleAddModule,
        handleUpdateModule,
        handleAddLesson,
        handleUpdateLesson,
        setTitle,
        setDescription,
        setContentType,
        setContent,
        setQuizQuestions,
        resetError
    };
};