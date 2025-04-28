import React from 'react';
import "./MySidebar.css"
import {Link} from "react-router-dom";
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faChartSimple, faUser, faBook, faSquarePollVertical} from "@fortawesome/free-solid-svg-icons";

const MySidebar = () => {
    return (
        <div className="sidebar">
            <div className="sidebar__links">
                <ul className="sidebar__links-ul">
                    <header className="sidebar__title">Таблицы</header>
                    <li>
                        <Link to={'/admin/users'}><FontAwesomeIcon icon={faUser}/> Пользователи </Link>
                    </li>
                    <li>
                        <Link to={'/admin/analytics'}><FontAwesomeIcon icon={faChartSimple}/> Аналитика</Link>
                    </li>
                    <li>
                        <Link to={'/admin/courses'}><FontAwesomeIcon icon={faBook}/> Курсы</Link>
                    </li>
                    <li>
                        <Link to={'/admin/progress-report'}><FontAwesomeIcon icon={faSquarePollVertical}/> Успеваемость</Link>
                    </li>
                </ul>
            </div>
        </div>
    );
};

export default MySidebar;