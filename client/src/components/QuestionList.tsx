import React from 'react';
import { Box, List, ListItemButton, ListItemText, IconButton } from '@mui/material';
import { Delete } from '@mui/icons-material';
import { IQuizQuestion } from '../types/types';

interface QuestionListProps {
    questions: IQuizQuestion[];
    editingQuestionId: string | null;
    onSelectQuestion: (question: IQuizQuestion) => void;
    onDeleteQuestion: (questionId: string) => void;
}

const QuestionList: React.FC<QuestionListProps> = ({
                                                       questions,
                                                       editingQuestionId,
                                                       onSelectQuestion,
                                                       onDeleteQuestion,
                                                   }) => (
    <Box sx={{ width: '30%' }}>
        <List>
            {questions.map((q, index) => (
                <ListItemButton
                    key={q.id}
                    selected={editingQuestionId === q.id}
                    onClick={() => onSelectQuestion(q)}
                    sx={{
                        border: '1px solid #eee',
                        borderRadius: 1,
                        mb: 1,
                        bgcolor: q.options.some(o => o.isCorrect) ? '#f5f5f5' : '#fff8e1'
                    }}
                >
                    <ListItemText
                        primary={`Вопрос ${index + 1}`}
                        secondary={q.question || 'Новый вопрос'}
                        secondaryTypographyProps={{ noWrap: true }}
                    />
                    <IconButton
                        edge="end"
                        onClick={(e) => {
                            e.stopPropagation();
                            onDeleteQuestion(q.id);
                        }}
                    >
                        <Delete fontSize="small" />
                    </IconButton>
                </ListItemButton>
            ))}
        </List>
    </Box>
);

export default QuestionList;