import axios, {
    AxiosInstance,
    AxiosError,
    AxiosResponse,
    InternalAxiosRequestConfig
} from 'axios';

const apiClient: AxiosInstance = axios.create({
    baseURL: 'http://localhost:5000/api',
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    withCredentials: false,
});

apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
        const token = localStorage.getItem('token');
        if (token && config.headers) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error: AxiosError): Promise<AxiosError> => {
        return Promise.reject(error);
    }
);

apiClient.interceptors.response.use(
    (response: AxiosResponse): AxiosResponse => response,
    (error: AxiosError): Promise<AxiosError> => {
        if (error.response?.status === 401) {
            localStorage.removeItem('token');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default apiClient;

// export const fetchAllCourseData = async () => {
//     try {
//         const response = await apiClient.get('/courses/tree'); // Предполагаем, что есть такой endpoint
//         return response.data;
//     } catch (error) {
//         console.error('Error fetching course data:', error);
//         throw error;
//     }
// };
// export const fetchAllCourseData = async () => {
//     try {
//         // Параллельно запрашиваем все необходимые данные
//         const [coursesRes, modulesRes, lessonsRes, contentsRes, questionsRes, optionsRes] = await Promise.all([
//             apiClient.get('/courses?is_deleted=false'),
//             apiClient.get('/modules?is_deleted=false'),
//             apiClient.get('/lessons?is_deleted=false'),
//             apiClient.get('/lesson-contents?is_deleted=false'),
//             apiClient.get('/quiz-questions?is_deleted=false'),
//             apiClient.get('/quiz-options?is_deleted=false'),
//         ]);
//
//         return {
//             courses: coursesRes.data,
//             modules: modulesRes.data,
//             lessons: lessonsRes.data,
//             contents: contentsRes.data,
//             questions: questionsRes.data,
//             options: optionsRes.data,
//         };
//     } catch (error) {
//         console.error('Error fetching all course data:', error);
//         throw error;
//     }
// };