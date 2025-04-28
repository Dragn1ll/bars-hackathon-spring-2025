import React from 'react';
import MyButton from "../components/UI/button/MyButton";
import {Link, NavLink} from "react-router-dom";
import MyNotification from "../components/UI/notification/MyNotification";
import MyNavbar from "../components/UI/navbar/MyNavbar";

const HomePage = () => {
    return (
        <>
            <MyNavbar title={'Главная страница'}/>
            <MyNotification message={"С возвращением"}/>
            <div style={{width:'25%', marginLeft:'auto', marginRight:'auto'}}>
                <Link to={""}>
                    <MyButton style={{marginTop: "300px"}}>Пройти тестирование</MyButton>
                </Link>
            </div>
        </>
    );
};

export default HomePage;