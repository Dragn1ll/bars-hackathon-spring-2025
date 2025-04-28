import { useState } from 'react';
import { IQuizQuestion, IAnswerOption } from '../types/types';

const useQuizEditor = (initialQuestions: IQuizQuestion[], onQuestionsChange: (questions: IQuizQuestion[]) => void) => {
    const [editingQuestion, setEditingQuestion] = useState<IQuizQuestion | null>(null);
    const [deleteConfirmation, setDeleteConfirmation] = useState<{
        open: boolean;
        questionId: string | null;
    }>({ open: false, questionId: null });

    const handleAddQuestion = () => {
        const newQuestion: IQuizQuestion = {
            id: `q-${Date.now()}`,
            question: '',
            options: [
                { id: `o-${Date.now()}-1`, text: '', isCorrect: false },
                { id: `o-${Date.now()}-2`, text: '', isCorrect: false }
            ],
        };
        onQuestionsChange([...initialQuestions, newQuestion]);
        setEditingQuestion(newQuestion);
    };

    const handleQuestionChange = (id: string, field: keyof IQuizQuestion, value: string) => {
        const updated = initialQuestions.map(q =>
            q.id === id ? { ...q, [field]: value } : q
        );
        onQuestionsChange(updated);
        if (editingQuestion?.id === id) {
            setEditingQuestion(updated.find(q => q.id === id) || null);
        }
    };

    const handleOptionChange = (questionId: string, optionId: string, text: string) => {
        const updated = initialQuestions.map(q => {
            if (q.id !== questionId) return q;
            return {
                ...q,
                options: q.options.map(o =>
                    o.id === optionId ? { ...o, text } : o
                )
            };
        });
        onQuestionsChange(updated);
    };

    const handleToggleCorrect = (questionId: string, optionId: string) => {
        const updated = initialQuestions.map(q => {
            if (q.id !== questionId) return q;
            return {
                ...q,
                options: q.options.map(o =>
                    o.id === optionId ? { ...o, isCorrect: !o.isCorrect } : o
                )
            };
        });
        onQuestionsChange(updated);
    };

    const handleAddOption = (questionId: string) => {
        const updated = initialQuestions.map(q => {
            if (q.id !== questionId) return q;
            return {
                ...q,
                options: [
                    ...q.options,
                    { id: `o-${Date.now()}`, text: '', isCorrect: false }
                ]
            };
        });
        onQuestionsChange(updated);
    };

    const handleDeleteOption = (questionId: string, optionId: string) => {
        const updated = initialQuestions.map(q => {
            if (q.id !== questionId) return q;
            return {
                ...q,
                options: q.options.filter(o => o.id !== optionId)
            };
        });
        onQuestionsChange(updated);
    };

    const handleDeleteQuestion = (questionId: string) => {
        const updated = initialQuestions.filter(q => q.id !== questionId);
        onQuestionsChange(updated);
        if (editingQuestion?.id === questionId) {
            setEditingQuestion(null);
        }
    };

    return {
        editingQuestion,
        handleAddQuestion,
        handleQuestionChange,
        handleOptionChange,
        handleToggleCorrect,
        handleAddOption,
        handleDeleteOption,
        handleDeleteQuestion,
        setEditingQuestion,
    };
};

export default useQuizEditor;