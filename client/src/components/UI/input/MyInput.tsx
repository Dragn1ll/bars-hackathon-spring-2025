import React, {FC} from 'react';
import './MyInput.css';

interface IMyInputProps {
    title: string;
    type: string;
    value: string;
    name: string;
    onChange?: (event: React.ChangeEvent<HTMLInputElement>) => void;
    required?: boolean;
}

const MyInput: FC<IMyInputProps> = ({title, type, required=false, value='', name, onChange}) => {
    return (
        <div className="myDivInput">
            <input type={type} required={required} name={name} value={value} onChange={onChange}/>
            <span>{title}</span>
        </div>
    );
};

export default MyInput;