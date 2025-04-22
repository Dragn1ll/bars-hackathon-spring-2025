import React from 'react';
import MyButton from "../components/UI/button/MyButton";
import {Link, NavLink} from "react-router-dom";
import MyNotification from "../components/UI/notification/MyNotification";

const HomePage = () => {
    return (
        <div>
            <MyNotification message={"С возращением Азамат"}/>
            <Link to={""}>
                <MyButton style={{marginTop: "300px"}}>Пройти тестирование</MyButton>
            </Link>
        </div>
    );
};

export default HomePage;