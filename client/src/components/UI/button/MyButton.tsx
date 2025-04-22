import React, {CSSProperties, FC} from 'react';
import  "./MyButton.css";

interface IMyButtonProps {
    style?: CSSProperties;
    type?: "button" | "submit";
    children?: React.ReactNode;
}

const MyButton: FC<IMyButtonProps> =
    ({children, type, style}) => {
    return (
        <button className="myButton" type={type} style={style}>
            {children}
        </button>
    );
};

export default MyButton;