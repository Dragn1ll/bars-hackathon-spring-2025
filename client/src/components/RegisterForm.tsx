import React, {FC, useState} from 'react';
import '../styles/AuthForm.css'
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {Link, NavLink, useNavigate} from "react-router-dom";
import MyButton from "./UI/button/MyButton";
import MyInput from "./UI/input/MyInput";
import {faArrowLeft} from "@fortawesome/free-solid-svg-icons";

interface IRegisterFormProps {
    title: string;
}

const RegisterForm: FC<IRegisterFormProps> = ({title}) => {
    const [formRegisterData, setFormRegisterData] = useState({
        name: "",
        surname: "",
        email: "",
        password1: "",
        password2: "",

    });
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const {name, value} = event.target;
        setFormRegisterData((prev) => ({
            ...prev, [name]: value})
        );
    };
    const navigate = useNavigate();
    return (
        <div className="contact-wrapper">
            <header className="login-cta">
                <FontAwesomeIcon icon={faArrowLeft} size="xl"
                                 onClick={() => navigate(-1)}
                                 style={{cursor: "pointer"}}/>
                <h2>{title}</h2>
            </header>
            <form>
                <MyInput title="Имя" type="text" required={true} value={formRegisterData.name}
                         name="name" onChange={handleChange} />
                <MyInput title="Фамилия" type="text" required={true} value={formRegisterData.surname}
                         name="surname" onChange={handleChange}/>
                <MyInput title="Адрес электронной почты" type="text" required={true} value={formRegisterData.email}
                         name="email" onChange={handleChange}/>
                <MyInput title="Пароль" type="password" required={true} value={formRegisterData.password1}
                         name="password1" onChange={handleChange}/>
                <MyInput title="Подтвердите пароль" type="password" required={true} value={formRegisterData.password2}
                         name="password2" onChange={handleChange}/>
                <MyButton type="submit" style={{marginTop: "12px"}}>Зарегистрироваться</MyButton>
            </form>
            <div className="auth__links" style={{justifyContent: "center"}}>
                Уже зарегистрировались?<Link to={'/login'}>Войти</Link>
            </div>
        </div>
    );
};

export default RegisterForm;