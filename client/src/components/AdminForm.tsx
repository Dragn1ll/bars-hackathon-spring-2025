import React, {FC, useState} from 'react';
import '../styles/AuthForm.css'
import {Link, NavLink, useNavigate} from "react-router-dom";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import {IErrorResponse, ILoginResponse} from "../types/types";
import axios, {AxiosError} from "axios";

interface ILoginProps {
    title: string;
}

const AdminForm: FC<ILoginProps> = ({title}) => {
    const navigate = useNavigate();
    const [formLoginData, setFormLoginData] = useState({
        email: "",
        password: "",
    });
    const [error, setError] = useState<string | null>(null);
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target;
        setFormLoginData((prev) => ({
            ...prev, [name]: value})
        );
    };
    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);

        try {
            const { data } = await axios.post<ILoginResponse>(
                'http://localhost:5000/api/login',
                {
                    email: formLoginData.email,
                    password: formLoginData.password
                },
                {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                }
            );

            console.log('Успешный вход:', data);
            localStorage.setItem('token', data.token);
            setFormLoginData({ email: "", password: "" });
            navigate('/admin');

        } catch (err) {
            const error = err as AxiosError<IErrorResponse>;
            if (error.response) {
                const errorMessage = error.response.data.message ||
                    (error.response.data.errors
                        ? Object.values(error.response.data.errors).flat().join(', ')
                        : 'Ошибка авторизации');
                setError(errorMessage);
            } else if (error.request) {
                setError('Сервер не отвечает');
            } else {
                setError('Ошибка при отправке запроса');
            }
            console.error('Ошибка:', error);
        }
    };
    return (
        <div className="contact-wrapper">
            <header className="login-cta">
                <h2>{title}</h2>
            </header>
            <form onSubmit={handleSubmit}>
                <MyInput title={"Адрес электронной почты"}
                         type="text"
                         name="email"
                         required={true}
                         value={formLoginData.email}
                         onChange={handleChange}
                />
                <MyInput title={"Пароль"}
                         type="password"
                         name="password"
                         required={true}
                         value={formLoginData.password}
                         onChange={handleChange}

                />
                <MyButton type="submit" style={{marginTop: "12px"}}>Войти</MyButton>
            </form>
            <div className="auth__links" style={{justifyContent: "center"}}>
                <Link to={''}>Забыли пароль?</Link>
            </div>
        </div>
    );
};

export default AdminForm;