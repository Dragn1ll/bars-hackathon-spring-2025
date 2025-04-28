import React, {CSSProperties, FC, useEffect, useState} from 'react';
import "./MyNotification.css"
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faFaceSmileWink} from "@fortawesome/free-solid-svg-icons";

interface IMyNotificationProps {
    message: string;
    style?: CSSProperties;
}

const MyNotification: FC<IMyNotificationProps> = ({message, style}) => {
    const [isVisible, setIsVisible] = useState(true);

    useEffect(() => {
        const timer = setTimeout(() => {
            setIsVisible(false);
        }, 2000);

        return () => clearTimeout(timer);
    }, []);

    if (!isVisible) return null;

    return (
        <div id="myNotification" style={style}>
            {message} <FontAwesomeIcon icon={faFaceSmileWink} style={{color: "#fbfb00"}}/>
        </div>
    );
};

export default MyNotification;