import React, {ReactElement} from "react";
import {AxiosError, AxiosResponse} from "axios";

export interface IQuizOption {
    id: number;
    question_id: number;
    text: string;
    is_correct: boolean;
    is_deleted?: boolean;
}
export interface IUsersListProps {
    users: IUser[];
    onSelect: (userId: number) => void;
    loading?: boolean;
}

export interface IUser {
    progress: number;
    avatar: string | undefined;
    id: number;
    username: string;
    name: string;
    surname: string;
    email?: string;
    role?: string;
    phone?: string;
    age?: number;
    joinDate?: string;
    lastLogin?: string;
    isDeleted?: boolean;
    courses?: {
        id: string;
        name: string;
        score: number;
        completed: boolean;
    }[];
    stats?: {
        totalCourses: number;
        completedCourses: number;
        averageScore: number;
    };
}

export interface IAuthUser {
    id: number;
    roles: string[];
}

export interface IUserCreateDialogProps {
    open: boolean;
    onClose: () => void;
    onSubmit: (user: any) => Promise<boolean>;
}

export interface ITableUsersContext {
    users: IUser[];
    loading: boolean;
    error: string | null;

    addUser: (user: Omit<IUser, 'id'>) => Promise<boolean>;
    deleteUser: (id: number) => Promise<boolean>;
    updateUser: (id: number, userData: Partial<IUser>) => Promise<boolean>;
    fetchUsers: () => Promise<void>;

    softDeleteUser: (id: number) => Promise<boolean>;
    restoreUser: (id: number) => Promise<boolean>;

    getDeletedUsers: () => Promise<IUser[]>;
    refreshUsers: () => void;
}

export interface IUserActionHandlers {
    onAddUser: (user: Omit<IUser, 'id'>) => Promise<boolean>;
    onDeleteUser: (id: number) => Promise<boolean>;
    onSoftDeleteUser: (id: number) => Promise<boolean>;
    onRestoreUser: (id: number) => Promise<boolean>;
    onUpdateUser: (id: number, userData: Partial<IUser>) => Promise<boolean>;
}

export interface Route {
    path: string;
    element: ReactElement;
}

export interface ILesson {
    id: string;
    title: string;
    content: ILessonContent;
}

export interface IModule {
    id: string;
    title: string;
    lessons: ILesson[];
    submodules?: IModule[];
}

export interface ICourse {
    id: string;
    title: string;
    description?: string;
    modules: IModule[];
}

export interface ICourseStructureProps {
    course: ICourse;
    onUpdateCourse: (course: ICourse) => void;
}

export interface ICourseDialogProps {
    open: boolean,
    type?: 'course' | 'module' | 'lesson' | null,
    context?: { courseId?: string; moduleId?: string } | null,
    editItem?: ICourse | IModule | ILesson | null,
    title?: string,
    description?: string,
    contentType?: 'text' | 'video' | 'quiz',
    content?: string,
    quizQuestions?: any[],
    onClose: () => void,
    onTitleChange?: (e: React.ChangeEvent<HTMLInputElement>) => void,
    onDescriptionChange?: (e: React.ChangeEvent<HTMLInputElement>) => void,
    onContentTypeChange?: (value: 'text' | 'video' | 'quiz') => void,
    onContentChange?: (e: React.ChangeEvent<HTMLInputElement>) => void,
    onQuizQuestionsChange?: (questions: any[]) => void,
    onSubmit?: () => void,
    loading?: boolean,
    onAdd?: (title: string, description?: string) => void
}

export interface IModuleCreateDialogProps {
    open: boolean;
    onClose: () => void;
    title: string;
    value: string;
    onChange: (value: string) => void;
    onAdd: () => void;
}

export interface ICoursesListProps {
    courses: ICourse[];
    onUpdateCourse: (updatedCourse: ICourse[]) => void;
    onDeleteCourse?: (courseId: string) => void;
}

export interface ILessonCreateDialogProps {
    open: boolean;
    onClose: () => void;
    onSuccess: (newLesson: ILesson) => void;
    courseId: string;
    modulePath: string[];
}

export interface ILessonEditorProps {
    lesson: ILesson;
    onSave: (updatedLesson: ILesson) => void;
    onCancel: () => void;
}

export interface IAnswerOption {
    id: string;
    text: string;
    isCorrect: boolean;
}

export interface ITextLessonContent {
    type: 'text';
    content: string;
}

export interface IVideoLessonContent {
    type: 'video';
    url: string;
    duration: number;
}

export interface IQuizLessonContent {
    type: 'quiz';
    questions: IQuizQuestion[];
}

export type ILessonContent = ITextLessonContent | IVideoLessonContent | IQuizLessonContent;

export interface IUserProgress {
    user: (value: IUserProgress, index: number, array: IUserProgress[]) => IUser;
    userId: string;
    courseId: string;
    progress: number;
    lastActivity: Date;
    modules: {
        [moduleId: string]: {
            progress: number;
            lessonsCompleted: number;
            testsCompleted: number;
        };
    };
}
export interface ITest {
    id: string;
    title: string;
    score?: number;
    maxScore: number;
    attempts: ITestAttempt[];
    isPassed?: boolean;
}

export interface ITestAttempt {
    id: string;
    date: Date;
    score: number;
    answers: {
        questionId: string;
        isCorrect: boolean;
        userAnswer: string;
    }[];
}
export interface IProgressData {
    courses: ICourse[];
    userProgress: IUserProgress[];
    selectedUserProgress?: IUserProgress & {
        user: IUser;
        detailedProgress: {
            modules: {
                id: string;
                name: string;
                progress: number;
                lessons: {
                    id: string;
                    title: string;
                    isCompleted: boolean;
                    completionDate?: Date;
                }[];
                tests: {
                    id: string;
                    title: string;
                    score?: number;
                    isPassed: boolean;
                    attempts: ITestAttempt[];
                }[];
            }[];
        };
    };
}
export interface IApiError {
    message: string;
    code?: string;
}

// @ts-ignore
export interface IAxiosApiError<T = any> extends AxiosError<T> {
    response?: AxiosResponse<{
        error?: IApiError;
        message?: string;
    }, any> & {
        config: any;
        headers: any;
        status: number;
        statusText: string;
    };
}

export interface IAnswerOption {
    id: string;
    text: string;
    isCorrect: boolean;
}

export interface IQuizQuestion {
    id: string;
    question: string;
    options: IAnswerOption[];
}

export interface IQuizEditorProps {
    questions: IQuizQuestion[];
    onQuestionsChange: (questions: IQuizQuestion[]) => void;
}

export interface IQuestionEditorProps {
    question: IQuizQuestion;
    onQuestionChange: (field: keyof IQuizQuestion, value: string) => void;
    onOptionChange: (optionId: string, text: string) => void;
    onToggleCorrect: (optionId: string) => void;
    onAddOption: () => void;
    onDeleteOption: (optionId: string) => void;
}

export interface IConfirmDialogProps {
    open: boolean;
    title: string;
    message: string;
    onConfirm: () => void;
    onCancel: () => void;
    confirmText?: string;
    cancelText?: string
    confirmDisabled?: boolean;
}

export interface IProtectedRouteProps {
    isAllowed: boolean;
    redirectPath?: string;
    children?: React.ReactNode;
}

export interface ILoginProps {
    title: string;
}

export interface ILoginFormData {
    email: string;
    password: string;
}

export interface ILoginResponse {
    token: string;
    user?: {
        id: string;
        email: string;
        name: string;
    };
}

export interface IErrorResponse {
    message: string;
    errors?: Record<string, string[]>;
}
export interface IMySpinnerProps {
    size?: number;
    color?: string;
    className?: string;
}

export interface IProgress {
    userId: string;
    courseId: string;
    moduleId: string;
    score: number;
    completionDate: Date;
    status: 'started' | 'completed' | 'failed';
}

