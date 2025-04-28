import React, { useState } from 'react';
import {
    Box,
    Tabs,
    Tab,
    TextField,
    Button,
    Typography,
    Divider,
    IconButton
} from '@mui/material';
import { Delete } from '@mui/icons-material';
import { ILesson, ILessonContent, IQuizQuestion, ILessonEditorProps } from '../types/types';

const LessonEditor: React.FC<ILessonEditorProps> = ({ lesson, onSave, onCancel }) => {
    const [activeTab, setActiveTab] = useState<'video' | 'text' | 'quiz'>(
        lesson.content.type
    );
    const [editedLesson, setEditedLesson] = useState<ILesson>(lesson);

    const handleContentChange = (content: ILessonContent) => {
        setEditedLesson(prev => ({ ...prev, content }));
    };

    const handleQuizQuestionChange = (index: number, question: IQuizQuestion) => {
        if (editedLesson.content.type !== 'quiz') return;

        const newQuestions = [...editedLesson.content.questions];
        newQuestions[index] = question;
        handleContentChange({ ...editedLesson.content, questions: newQuestions });
    };

    const addQuizQuestion = () => {
        if (editedLesson.content.type !== 'quiz') return;

        const timestamp = Date.now();
        const newQuestion: IQuizQuestion = {
            id: `q-${timestamp}`,
            question: '',
            options: [
                { id: `opt-${timestamp}-1`, text: '', isCorrect: true },
                { id: `opt-${timestamp}-2`, text: '', isCorrect: false },
                { id: `opt-${timestamp}-3`, text: '', isCorrect: false }
            ]
        };
        handleContentChange({
            ...editedLesson.content,
            questions: [...editedLesson.content.questions, newQuestion]
        });
    };

    return (
        <Box sx={{ width: '100%' }}>
            <TextField
                label="Название урока"
                fullWidth
                value={editedLesson.title}
                onChange={(e) => setEditedLesson(prev => ({ ...prev, title: e.target.value }))}
                sx={{ mb: 3 }}
            />

            <Tabs value={activeTab} onChange={(e, newValue) => setActiveTab(newValue)}>
                <Tab label="Видео" value="video" />
                <Tab label="Текст" value="text" />
                <Tab label="Тест" value="quiz" />
            </Tabs>

            <Box sx={{ pt: 2 }}>
                {activeTab === 'video' && (
                    <VideoEditor
                        content={editedLesson.content.type === 'video' ? editedLesson.content : null}
                        onChange={(video) => handleContentChange(video)}
                    />
                )}

                {activeTab === 'text' && (
                    <TextEditor
                        content={editedLesson.content.type === 'text' ? editedLesson.content : null}
                        onChange={(text) => handleContentChange(text)}
                    />
                )}

                {activeTab === 'quiz' && (
                    <QuizEditor
                        content={editedLesson.content.type === 'quiz' ? editedLesson.content : null}
                        onChange={(quiz) => handleContentChange(quiz)}
                        onQuestionChange={handleQuizQuestionChange}
                        onAddQuestion={addQuizQuestion}
                    />
                )}
            </Box>

            <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3, gap: 1 }}>
                <Button variant="outlined" onClick={onCancel}>
                    Отмена
                </Button>
                <Button
                    variant="contained"
                    onClick={() => onSave(editedLesson)}
                    disabled={!editedLesson.title.trim()}
                >
                    Сохранить
                </Button>
            </Box>
        </Box>
    );
};

const VideoEditor: React.FC<{
    content: { type: 'video'; url: string; duration: number } | null;
    onChange: (content: { type: 'video'; url: string; duration: number }) => void;
}> = ({ content, onChange }) => {
    const [video, setVideo] = useState({
        url: content?.url || '',
        duration: content?.duration || 0
    });

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <TextField
                label="URL видео"
                fullWidth
                value={video.url}
                onChange={(e) => {
                    setVideo(prev => ({ ...prev, url: e.target.value }));
                    onChange({ type: 'video', url: e.target.value, duration: video.duration });
                }}
            />
            <TextField
                label="Длительность (минуты)"
                type="number"
                value={video.duration}
                onChange={(e) => {
                    const duration = parseInt(e.target.value) || 0;
                    setVideo(prev => ({ ...prev, duration }));
                    onChange({ type: 'video', url: video.url, duration });
                }}
            />
        </Box>
    );
};

const TextEditor: React.FC<{
    content: { type: 'text'; content: string } | null;
    onChange: (content: { type: 'text'; content: string }) => void;
}> = ({ content, onChange }) => {
    const [text, setText] = useState(content?.content || '');

    return (
        <TextField
            label="Текст урока"
            multiline
            rows={10}
            fullWidth
            value={text}
            onChange={(e) => {
                setText(e.target.value);
                onChange({ type: 'text', content: e.target.value });
            }}
        />
    );
};

const QuizEditor: React.FC<{
    content: { type: 'quiz'; questions: IQuizQuestion[] } | null;
    onChange: (content: { type: 'quiz'; questions: IQuizQuestion[] }) => void;
    onQuestionChange: (index: number, question: IQuizQuestion) => void;
    onAddQuestion: () => void;
}> = ({ content, onChange, onQuestionChange, onAddQuestion }) => {
    const questions = content?.questions || [];

    const handleOptionCorrectChange = (qIndex: number, optionId: string) => {
        const question = questions[qIndex];
        const updatedOptions = question.options.map(opt => ({
            ...opt,
            isCorrect: opt.id === optionId
        }));

        onQuestionChange(qIndex, {
            ...question,
            options: updatedOptions
        });
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
            {questions.map((q, qIndex) => (
                <Box key={q.id} sx={{ p: 2, border: '1px solid #ddd', borderRadius: 1 }}>
                    <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 1 }}>
                        <Typography variant="subtitle1">Вопрос {qIndex + 1}</Typography>
                        <IconButton
                            size="small"
                            onClick={() => {
                                const newQuestions = questions.filter((_, i) => i !== qIndex);
                                onChange({ type: 'quiz', questions: newQuestions });
                            }}
                        >
                            <Delete fontSize="small" />
                        </IconButton>
                    </Box>

                    <TextField
                        label="Текст вопроса"
                        fullWidth
                        value={q.question}
                        onChange={(e) => onQuestionChange(qIndex, { ...q, question: e.target.value })}
                        sx={{ mb: 2 }}
                    />

                    {q.options.map((option) => (
                        <Box key={option.id} sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                            <TextField
                                label={`Вариант ответа`}
                                fullWidth
                                value={option.text}
                                onChange={(e) => {
                                    const newOptions = q.options.map(opt =>
                                        opt.id === option.id ? { ...opt, text: e.target.value } : opt
                                    );
                                    onQuestionChange(qIndex, { ...q, options: newOptions });
                                }}
                            />
                            <Button
                                size="small"
                                color={option.isCorrect ? 'primary' : 'inherit'}
                                onClick={() => handleOptionCorrectChange(qIndex, option.id)}
                                sx={{ ml: 1 }}
                            >
                                Верный
                            </Button>
                        </Box>
                    ))}
                </Box>
            ))}

            <Button variant="outlined" onClick={onAddQuestion}>
                Добавить вопрос
            </Button>
        </Box>
    );
};
export default LessonEditor;