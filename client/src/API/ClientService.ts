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