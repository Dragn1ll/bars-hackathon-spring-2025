import React from 'react';
import { Box, Button } from '@mui/material';
import { Add } from '@mui/icons-material';
import QuestionList from './QuestionList';
import QuestionEditor from './QuestionEditor';
import useQuizEditor from '../../hooks/useQuizEditor';
import { IQuizEditorProps } from '../../types/types';

const QuizEditor: React.FC<IQuizEditorProps> = ({ questions, onQuestionsChange }) => {
    const {
        editingQuestion,
        handleAddQuestion,
        handleQuestionChange,
        handleOptionChange,
        handleToggleCorrect,
        handleAddOption,
        handleDeleteOption,
        handleDeleteQuestion,
        setEditingQuestion,
    } = useQuizEditor(questions, onQuestionsChange);

    return (
        <Box sx={{ mt: 2 }}>
            <Button
                variant="outlined"
                startIcon={<Add />}
                onClick={handleAddQuestion}
                sx={{ mb: 3 }}
            >
                Добавить вопрос
            </Button>

            <Box sx={{ display: 'flex', gap: 3 }}>
                <QuestionList
                    questions={questions}
                    editingQuestionId={editingQuestion?.id || null}
                    onSelectQuestion={setEditingQuestion}
                    onDeleteQuestion={handleDeleteQuestion}
                />

                {editingQuestion && (
                    <QuestionEditor
                        question={editingQuestion}
                        onQuestionChange={(field, value) =>
                            handleQuestionChange(editingQuestion.id, field, value)
                        }
                        onOptionChange={(optionId, text) =>
                            handleOptionChange(editingQuestion.id, optionId, text)
                        }
                        onToggleCorrect={(optionId) =>
                            handleToggleCorrect(editingQuestion.id, optionId)
                        }
                        onAddOption={() => handleAddOption(editingQuestion.id)}
                        onDeleteOption={(optionId) =>
                            handleDeleteOption(editingQuestion.id, optionId)
                        }
                    />
                )}
            </Box>
        </Box>
    );
};

export default QuizEditor;