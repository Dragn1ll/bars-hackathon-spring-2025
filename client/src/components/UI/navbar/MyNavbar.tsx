import React from 'react';
import "./MyNavbar.css";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faHome, faUser, faMagnifyingGlass, faLayerGroup} from "@fortawesome/free-solid-svg-icons";
import {Link, NavLink} from "react-router-dom";
import MyInput from "../input/MyInput";
import MyButton from "../button/MyButton";


const MyNavbar = () => {
    return (
        <div className="navbar">
            <div className="navbar__links">
                <h3>Панель администрирования</h3>
            </div>
        </div>
    );
};

export default MyNavbar;