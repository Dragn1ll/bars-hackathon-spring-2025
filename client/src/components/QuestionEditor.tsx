import React from 'react';
import {
    Box,
    Typography,
    TextField,
    Button,
    IconButton,
    FormControlLabel,
    Checkbox
} from '@mui/material';
import { Add, Delete } from '@mui/icons-material';
import { IQuizQuestion, IQuestionEditorProps } from '../types/types';

const QuestionEditor: React.FC<IQuestionEditorProps> = ({
                                                           question,
                                                           onQuestionChange,
                                                           onOptionChange,
                                                           onToggleCorrect,
                                                           onAddOption,
                                                           onDeleteOption,
                                                       }) => (
    <Box sx={{ width: '70%', p: 2, border: '1px solid #eee', borderRadius: 1 }}>
        <TextField
            label="Текст вопроса"
            fullWidth
            value={question.question}
            onChange={(e) => onQuestionChange('question', e.target.value)}
            sx={{ mb: 3 }}
        />

        <Typography variant="subtitle1" sx={{ mb: 2 }}>
            Варианты ответов:
        </Typography>

        {question.options.map((option, idx) => (
            <Box
                key={option.id}
                sx={{
                    display: 'flex',
                    alignItems: 'center',
                    gap: 1,
                    mb: 2
                }}
            >
                <FormControlLabel
                    control={
                        <Checkbox
                            checked={option.isCorrect}
                            onChange={() => onToggleCorrect(option.id)}
                        />
                    }
                    label=""
                />
                <TextField
                    fullWidth
                    value={option.text}
                    onChange={(e) => onOptionChange(option.id, e.target.value)}
                    placeholder={`Вариант ${idx + 1}`}
                />
                <IconButton
                    onClick={() => onDeleteOption(option.id)}
                    disabled={question.options.length <= 2}
                >
                    <Delete fontSize="small" />
                </IconButton>
            </Box>
        ))}

        <Button
            startIcon={<Add />}
            onClick={onAddOption}
            sx={{ mb: 3 }}
        >
            Добавить вариант
        </Button>
    </Box>
);

export default QuestionEditor;