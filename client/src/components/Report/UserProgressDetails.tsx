// import React, { useState } from 'react';
// import {
//     Box,
//     Typography,
//     Paper,
//     Accordion,
//     AccordionSummary,
//     AccordionDetails,
//     LinearProgress,
//     Chip,
//     Table,
//     TableBody,
//     TableCell,
//     TableContainer,
//     TableHead,
//     TableRow,
//     Button,
//     CircularProgress,
//     Select,
//     MenuItem,
//     FormControl,
//     InputLabel,
//     Grid,
//     Avatar
// } from '@mui/material';
// import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
// import { ICourse, IUserProgressDetails } from "../../types/types";
// import MySpinner from "../UI/spinner/MySpinner";
//
// interface UserProgressDetailsProps {
//     progress: IUserProgressDetails | null;
//     courses: ICourse[];
//     loading?: boolean;
//     onBack?: () => void;
//     onCourseSelect?: (courseId: string) => void;
// }
//
// const UserProgressDetails = ({
//                                  progress,
//                                  courses,
//                                  loading,
//                                  onBack,
//                                  onCourseSelect
//                              }: UserProgressDetailsProps) => {
//     const [selectedCourseId, setSelectedCourseId] = useState<string>(
//         progress?.courseId || ''
//     );
//
//     const handleCourseChange = (event: React.ChangeEvent<{ value: unknown }>) => {
//         const courseId = event.target.value as string;
//         setSelectedCourseId(courseId);
//         if (onCourseSelect) {
//             onCourseSelect(courseId);
//         }
//     };
//
//     if (loading) return (
//         <Box
//             sx={{
//                 display: 'flex',
//                 justifyContent: 'center',
//                 alignItems: 'center',
//                 height: '100vh'
//             }}
//         >
//             <MySpinner />
//         </Box>
//     );
//
//     if (!progress) {
//         return (
//             <Box p={2}>
//                 <Typography>Данные о прогрессе не найдены</Typography>
//                 {onBack && (
//                     <Button variant="outlined" onClick={onBack} sx={{ mt: 2 }}>
//                         Назад к списку
//                     </Button>
//                 )}
//             </Box>
//         );
//     }
//
//     const selectedCourse = courses.find(c => c.id === selectedCourseId);
//     const userCourseProgress = progress.coursesProgress?.find(
//         cp => cp.courseId === selectedCourseId
//     );
//     console.log('Progress data:', {
//         progress,
//         hasCourses: !!progress?.coursesProgress,
//         coursesCount: progress?.coursesProgress?.length
//     });
//     return (
//         <Box sx={{ width: '100%' }}>
//             {/* Шапка с информацией о пользователе */}
//             <Box display="flex" alignItems="center" mb={3}>
//                 {onBack && (
//                     <Button
//                         variant="outlined"
//                         onClick={onBack}
//                         sx={{ mr: 2 }}
//                     >
//                         Назад
//                     </Button>
//                 )}
//                 <Avatar
//                     src="{progress.user.avatar}"
//                     sx={{ width: 100, height: 100, mr: 2 }}
//                 />
//                 <Box>
//                     <Typography variant="h5" component="div">
//                         {progress.user.name}
//                     </Typography>
//                     <Typography color="text.secondary">
//                         <a href={"https://t.me/" + progress.user.name}>Телеграмм аккаунт</a>
//                     </Typography>
//                 </Box>
//             </Box>
//
//             {/* Выбор курса */}
//             {progress?.coursesProgress?.length ? (
//                 <Box sx={{ mb: 4, width: '50%' }}>
//                     <FormControl fullWidth>
//                         <InputLabel id="course-select-label">Курс</InputLabel>
//                         <Select
//                             labelId="course-select-label"
//                             value={selectedCourseId}
//                             onChange={handleCourseChange}
//                             label="Курс"
//                             disabled={loading}
//                         >
//                             {progress.coursesProgress.map(cp => {
//                                 const course = courses.find(c => c.id === cp.courseId);
//                                 return (
//                                     <MenuItem key={cp.courseId} value={cp.courseId}>
//                                         {course?.title || `Курс ${cp.courseId}`} ({Math.round(cp.progress)}%)
//                                     </MenuItem>
//                                 );
//                             })}
//                         </Select>
//                     </FormControl>
//                 </Box>
//             ) : (
//                 <Typography color="text.secondary" sx={{ mb: 4 }}>
//                     Нет доступных курсов
//                 </Typography>
//             )}
//
//             {selectedCourse && userCourseProgress && (
//                 <>
//                     {/* Информация о курсе */}
//                     <Paper sx={{ p: 3, mb: 3 }}>
//                         <Grid container spacing={2}>
//                             <Grid item xs={12} md={6}>
//                                 <Typography variant="h6">{selectedCourse.title}</Typography>
//                                 <Typography color="text.secondary" sx={{ mb: 2 }}>
//                                     {selectedCourse.description}
//                                 </Typography>
//                             </Grid>
//                             <Grid item xs={12} md={6}>
//                                 <Box sx={{ display: 'flex', alignItems: 'center' }}>
//                                     <Typography sx={{ mr: 2 }}>Общий прогресс:</Typography>
//                                     <LinearProgress
//                                         variant="determinate"
//                                         value={userCourseProgress.progress}
//                                         sx={{ flexGrow: 1, height: 10 }}
//                                     />
//                                     <Typography sx={{ ml: 2, minWidth: 40 }}>
//                                         {Math.round(userCourseProgress.progress)}%
//                                     </Typography>
//                                 </Box>
//                                 <Typography sx={{ mt: 1 }}>
//                                     Последняя активность: {new Date(userCourseProgress.lastActivity).toLocaleDateString()}
//                                 </Typography>
//                             </Grid>
//                         </Grid>
//                     </Paper>
//
//                     {/* Прогресс по модулям */}
//                     <Typography variant="h6" sx={{ mb: 2 }}>
//                         Прогресс по модулям
//                     </Typography>
//
//                     {userCourseProgress.detailedProgress.modules.map(module => {
//                         const courseModule = selectedCourse.modules.find(m => m.id === module.id);
//
//                         return (
//                             <Accordion key={module.id} sx={{ mb: 2 }}>
//                                 <AccordionSummary expandIcon={<ExpandMoreIcon />}>
//                                     <Box sx={{ width: '100%', display: 'flex', alignItems: 'center' }}>
//                                         <Typography sx={{ flex: 1, fontWeight: 'medium' }}>
//                                             {courseModule?.title || module.id}
//                                         </Typography>
//                                         <LinearProgress
//                                             variant="determinate"
//                                             value={module.progress}
//                                             sx={{ width: 120, height: 8, mr: 2 }}
//                                         />
//                                         <Chip
//                                             label={`${Math.round(module.progress)}%`}
//                                             color={
//                                                 module.progress >= 80 ? 'success' :
//                                                     module.progress >= 50 ? 'warning' : 'error'
//                                             }
//                                         />
//                                     </Box>
//                                 </AccordionSummary>
//                                 <AccordionDetails>
//                                     {/* Уроки */}
//                                     <Typography variant="subtitle1" gutterBottom>
//                                         Уроки ({module.lessons.filter(l => l.isCompleted).length}/{module.lessons.length})
//                                     </Typography>
//                                     <TableContainer component={Paper} sx={{ mb: 3 }}>
//                                         <Table size="small">
//                                             <TableHead>
//                                                 <TableRow>
//                                                     <TableCell>Урок</TableCell>
//                                                     <TableCell width={120} align="right">Статус</TableCell>
//                                                     <TableCell width={150} align="right">Дата завершения</TableCell>
//                                                 </TableRow>
//                                             </TableHead>
//                                             <TableBody>
//                                                 {module.lessons.map(lesson => {
//                                                     const courseLesson = courseModule?.lessons.find(l => l.id === lesson.id);
//                                                     return (
//                                                         <TableRow key={lesson.id}>
//                                                             <TableCell>
//                                                                 {courseLesson?.title || lesson.id}
//                                                                 {courseLesson?.duration && (
//                                                                     <Typography variant="caption" color="text.secondary" sx={{ ml: 1 }}>
//                                                                         ({courseLesson.duration} мин)
//                                                                     </Typography>
//                                                                 )}
//                                                             </TableCell>
//                                                             <TableCell align="right">
//                                                                 <Chip
//                                                                     label={lesson.isCompleted ? 'Завершен' : 'Не завершен'}
//                                                                     color={lesson.isCompleted ? 'success' : 'default'}
//                                                                     size="small"
//                                                                 />
//                                                             </TableCell>
//                                                             <TableCell align="right">
//                                                                 {lesson.completionDate ?
//                                                                     new Date(lesson.completionDate).toLocaleDateString() : '-'}
//                                                             </TableCell>
//                                                         </TableRow>
//                                                     );
//                                                 })}
//                                             </TableBody>
//                                         </Table>
//                                     </TableContainer>
//
//                                     {/* Тесты */}
//                                     <Typography variant="subtitle1" gutterBottom>
//                                         Тесты ({module.tests.filter(t => t.isPassed).length}/{module.tests.length})
//                                     </Typography>
//                                     {module.tests.map(test => {
//                                         const courseTest = courseModule?.tests.find(t => t.id === test.id);
//                                         return (
//                                             <Accordion key={test.id} sx={{ mb: 2 }}>
//                                                 <AccordionSummary expandIcon={<ExpandMoreIcon />}>
//                                                     <Box sx={{ width: '100%', display: 'flex', alignItems: 'center' }}>
//                                                         <Typography sx={{ flex: 1 }}>
//                                                             {courseTest?.title || test.id}
//                                                         </Typography>
//                                                         <Chip
//                                                             label={test.isPassed ? 'Пройден' : 'Не пройден'}
//                                                             color={test.isPassed ? 'success' : 'error'}
//                                                             size="small"
//                                                             sx={{ mr: 1 }}
//                                                         />
//                                                         <Chip
//                                                             label={`Лучший результат: ${test.bestScore || 0}/${test.maxScore || 100}`}
//                                                             size="small"
//                                                         />
//                                                     </Box>
//                                                 </AccordionSummary>
//                                                 <AccordionDetails>
//                                                     <Typography variant="subtitle2" gutterBottom>
//                                                         Попытки ({test.attempts.length})
//                                                     </Typography>
//                                                     <TableContainer component={Paper}>
//                                                         <Table size="small">
//                                                             <TableHead>
//                                                                 <TableRow>
//                                                                     <TableCell>Дата</TableCell>
//                                                                     <TableCell align="right">Результат</TableCell>
//                                                                     <TableCell align="right">Баллы</TableCell>
//                                                                     <TableCell align="right">Статус</TableCell>
//                                                                 </TableRow>
//                                                             </TableHead>
//                                                             <TableBody>
//                                                                 {test.attempts.map(attempt => (
//                                                                     <TableRow key={attempt.id}>
//                                                                         <TableCell>
//                                                                             {new Date(attempt.date).toLocaleDateString()}
//                                                                         </TableCell>
//                                                                         <TableCell align="right">
//                                                                             {attempt.score}/{test.maxScore}
//                                                                         </TableCell>
//                                                                         <TableCell align="right">
//                                                                             {Math.round((attempt.score / test.maxScore) * 100)}%
//                                                                         </TableCell>
//                                                                         <TableCell align="right">
//                                                                             <Chip
//                                                                                 label={attempt.score >= test.passingScore ? 'Успешно' : 'Неудачно'}
//                                                                                 color={attempt.score >= test.passingScore ? 'success' : 'error'}
//                                                                                 size="small"
//                                                                             />
//                                                                         </TableCell>
//                                                                     </TableRow>
//                                                                 ))}
//                                                             </TableBody>
//                                                         </Table>
//                                                     </TableContainer>
//                                                 </AccordionDetails>
//                                             </Accordion>
//                                         );
//                                     })}
//                                 </AccordionDetails>
//                             </Accordion>
//                         );
//                     })}
//                 </>
//             )}
//         </Box>
//     );
// };
//
// export default UserProgressDetails;