import React, { useState } from 'react';
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    TextField,
    Box,
    Tabs,
    Tab,
    Typography,
    Chip,
    IconButton,
    Checkbox,
    FormControlLabel
} from '@mui/material';
import { Add, Delete, VideoLibrary, Article, Quiz } from '@mui/icons-material';
import useCourses from '../../hooks/useCourses';
import {
    ILesson,
    ILessonContent,
    IQuizQuestion,
    ITextLessonContent,
    IVideoLessonContent,
    IQuizLessonContent,
    ILessonCreateDialogProps
} from '../../types/types';

const LessonCreateDialog: React.FC<ILessonCreateDialogProps> = ({
                                                                    open,
                                                                    onClose,
                                                                    onSuccess,
                                                                    courseId,
                                                                    modulePath
                                                                }) => {
    const { loading, error, createLesson } = useCourses();
    const [activeTab, setActiveTab] = useState<'text' | 'video' | 'quiz'>('text');
    const [title, setTitle] = useState('');
    const [content, setContent] = useState<ILessonContent>({
        type: 'text',
        content: ''
    } as ITextLessonContent);
    const [quizQuestions, setQuizQuestions] = useState<IQuizQuestion[]>([]);

    const handleTabChange = (event: React.SyntheticEvent, newValue: 'text' | 'video' | 'quiz') => {
        setActiveTab(newValue);
        if (newValue === 'video') {
            setContent({
                type: 'video',
                url: '',
                duration: 0
            } as IVideoLessonContent);
        } else if (newValue === 'text') {
            setContent({
                type: 'text',
                content: ''
            } as ITextLessonContent);
        } else {
            setContent({
                type: 'quiz',
                questions: []
            } as IQuizLessonContent);
        }
    };

    const handleAddQuestion = () => {
        const newQuestion: IQuizQuestion = {
            id: `q-${Date.now()}`,
            question: '',
            options: [
                { id: `o-${Date.now()}-1`, text: '', isCorrect: false },
                { id: `o-${Date.now()}-2`, text: '', isCorrect: false }
            ],
        };
        setQuizQuestions([...quizQuestions, newQuestion]);
    };

    const handleQuestionChange = (index: number, field: keyof IQuizQuestion, value: any) => {
        const updatedQuestions = [...quizQuestions];
        updatedQuestions[index] = {
            ...updatedQuestions[index],
            [field]: value
        };
        setQuizQuestions(updatedQuestions);
    };

    const handleRemoveQuestion = (index: number) => {
        setQuizQuestions(quizQuestions.filter((_, i) => i !== index));
    };

    const handleSubmit = async () => {
        try {
            let lessonContent: ILessonContent;

            switch (activeTab) {
                case 'text':
                    lessonContent = {
                        type: 'text',
                        content: content.type === 'text' ? content.content : ''
                    } as ITextLessonContent;
                    break;
                case 'video':
                    lessonContent = {
                        type: 'video',
                        url: content.type === 'video' ? content.url : '',
                        duration: content.type === 'video' ? content.duration : 0
                    } as IVideoLessonContent;
                    break;
                case 'quiz':
                    lessonContent = {
                        type: 'quiz',
                        questions: quizQuestions
                    } as IQuizLessonContent;
                    break;
                default:
                    lessonContent = {
                        type: 'text',
                        content: ''
                    } as ITextLessonContent;
            }

            const newLesson: Omit<ILesson, 'id'> = {
                title,
                content: lessonContent
            };

            const createdLesson = await createLesson(
                courseId,
                modulePath[modulePath.length - 1],
                newLesson
            );

            onSuccess(createdLesson);
            onClose();
            resetForm();
        } catch (err) {
            console.error('Error creating lesson:', err);
        }
    };

    const resetForm = () => {
        setTitle('');
        setContent({
            type: 'text',
            content: ''
        } as ITextLessonContent);
        setActiveTab('text');
        setQuizQuestions([]);
    };

    const renderContentField = () => {
        switch (activeTab) {
            case 'text':
                return (
                    <TextField
                        label="Текст урока"
                        multiline
                        rows={10}
                        fullWidth
                        value={content.type === 'text' ? content.content : ''}
                        onChange={(e) => setContent({
                            type: 'text',
                            content: e.target.value
                        } as ITextLessonContent)}
                    />
                );
            case 'video':
                return (
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                        <TextField
                            label="URL видео"
                            fullWidth
                            value={content.type === 'video' ? content.url : ''}
                            onChange={(e) => setContent({
                                ...content as IVideoLessonContent,
                                url: e.target.value
                            })}
                        />
                        <TextField
                            label="Длительность (минуты)"
                            type="number"
                            value={content.type === 'video' ? content.duration : 0}
                            onChange={(e) => setContent({
                                ...content as IVideoLessonContent,
                                duration: parseInt(e.target.value) || 0
                            })}
                        />
                    </Box>
                );
            case 'quiz':
                return (
                    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
                        {quizQuestions.map((question, index) => (
                            <Box key={question.id} sx={{
                                p: 2,
                                border: '1px solid #eee',
                                borderRadius: 1
                            }}>
                                <Box sx={{
                                    display: 'flex',
                                    justifyContent: 'space-between',
                                    mb: 2
                                }}>
                                    <Typography variant="subtitle1">
                                        Вопрос {index + 1}
                                    </Typography>
                                    <IconButton
                                        size="small"
                                        onClick={() => handleRemoveQuestion(index)}
                                        color="error"
                                    >
                                        <Delete fontSize="small" />
                                    </IconButton>
                                </Box>

                                <TextField
                                    label="Текст вопроса"
                                    fullWidth
                                    value={question.question}
                                    onChange={(e) =>
                                        handleQuestionChange(index, 'question', e.target.value)
                                    }
                                    sx={{ mb: 2 }}
                                />

                                {question.options.map((option, optIndex) => (
                                    <Box
                                        key={option.id}
                                        sx={{
                                            display: 'flex',
                                            alignItems: 'center',
                                            mb: 1
                                        }}
                                    >
                                        <TextField
                                            label={`Вариант ${optIndex + 1}`}
                                            fullWidth
                                            value={option.text}
                                            onChange={(e) => {
                                                const newOptions = [...question.options];
                                                newOptions[optIndex].text = e.target.value;
                                                handleQuestionChange(index, 'options', newOptions);
                                            }}
                                        />
                                        <Checkbox
                                            checked={option.isCorrect}
                                            onChange={() => {
                                                const newOptions = [...question.options];
                                                newOptions[optIndex].isCorrect = !option.isCorrect;
                                                handleQuestionChange(index, 'options', newOptions);
                                            }}
                                            sx={{ ml: 1 }}
                                        />
                                        <IconButton
                                            onClick={() => {
                                                const newOptions = question.options.filter((_, i) => i !== optIndex);
                                                handleQuestionChange(index, 'options', newOptions);
                                            }}
                                            disabled={question.options.length <= 2}
                                        >
                                            <Delete fontSize="small" />
                                        </IconButton>
                                    </Box>
                                ))}

                                <Button
                                    variant="outlined"
                                    startIcon={<Add />}
                                    onClick={() => {
                                        const newOptions = [...question.options, {
                                            id: `opt-${Date.now()}`,
                                            text: '',
                                            isCorrect: false
                                        }];
                                        handleQuestionChange(index, 'options', newOptions);
                                    }}
                                    sx={{ mb: 2 }}
                                >
                                    Добавить вариант
                                </Button>

                                {/*<TextField*/}
                                {/*    label="Объяснение (опционально)"*/}
                                {/*    fullWidth*/}
                                {/*    multiline*/}
                                {/*    rows={2}*/}
                                {/*    value={question.explanation || ''}*/}
                                {/*    onChange={(e) =>*/}
                                {/*        handleQuestionChange(index, 'explanation', e.target.value)*/}
                                {/*    }*/}
                                {/*/>*/}
                            </Box>
                        ))}

                        <Button
                            variant="outlined"
                            startIcon={<Add />}
                            onClick={handleAddQuestion}
                        >
                            Добавить вопрос
                        </Button>
                    </Box>
                );
            default:
                return null;
        }
    };

    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth="md"
            fullWidth
        >
            <DialogTitle>Создание нового урока</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Название урока"
                    fullWidth
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                    sx={{ mb: 3 }}
                />

                <Tabs value={activeTab} onChange={handleTabChange}>
                    <Tab label="Текст" value="text" icon={<Article />} />
                    <Tab label="Видео" value="video" icon={<VideoLibrary />} />
                    <Tab label="Тест" value="quiz" icon={<Quiz />} />
                </Tabs>

                <Box sx={{ pt: 2 }}>
                    {renderContentField()}
                </Box>

                {error && (
                    <Typography color="error" sx={{ mt: 2 }}>
                        {error}
                    </Typography>
                )}
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={loading}>
                    Отмена
                </Button>
                <Button
                    onClick={handleSubmit}
                    disabled={loading || !title.trim()}
                    variant="contained"
                >
                    {loading ? 'Создание...' : 'Создать'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default LessonCreateDialog;