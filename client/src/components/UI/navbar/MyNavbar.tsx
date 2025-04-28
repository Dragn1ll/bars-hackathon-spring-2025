import React, { FC, useState, useRef, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCog, faHome, faSignOutAlt } from '@fortawesome/free-solid-svg-icons';
import './MyNavbar.css';

interface IMyNavbarProps {
    title?: string;
}

const MyNavbar: FC<IMyNavbarProps> = ({ title = 'Панель администрирования' }) => {
    const [showDropdown, setShowDropdown] = useState(false);
    const menuRef = useRef<HTMLDivElement>(null);
    const location = useLocation();

    useEffect(() => {
        const handleClickOutside = (e: MouseEvent) => {
            if (menuRef.current && !menuRef.current.contains(e.target as Node)) {
                setShowDropdown(false);
            }
        };

        document.addEventListener('mousedown', handleClickOutside);
        return () => document.removeEventListener('mousedown', handleClickOutside);
    }, []);

    const isAdminPage = location.pathname.startsWith('/admin');
    const isHomePage = location.pathname === '/';

    return (
        <div className="navbar" style={{ position: 'relative', overflow: 'visible' }}>
            <div className="navbar__links" style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                <h3>{title}</h3>

                <div ref={menuRef} style={{ marginLeft: 'auto', position: 'relative' }}>
                    <span
                        onClick={() => setShowDropdown(!showDropdown)}
                        style={{ padding: '8px 12px' }}
                    >
                        <FontAwesomeIcon icon={faCog} />
                    </span>

                    {showDropdown && (
                        <div
                            style={{
                                position: 'absolute',
                                right: 0,
                                top: '100%',
                                backgroundColor: 'white',
                                border: '1px solid #ddd',
                                borderRadius: '4px',
                                padding: '8px',
                                zIndex: 1000,
                                minWidth: '200px',
                                boxShadow: '0 2px 10px rgba(0,0,0,0.15)',
                            }}
                        >
                            {isAdminPage ? (
                                <Link
                                    to="/"
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '8px',
                                        padding: '8px',
                                        textDecoration: 'none',
                                        color: '#333',
                                    }}
                                    onClick={() => setShowDropdown(false)}
                                >
                                    <FontAwesomeIcon icon={faSignOutAlt} />
                                    Выйти из админки
                                </Link>
                            ) : isHomePage ? (
                                <Link
                                    to="/admin"
                                    style={{
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '8px',
                                        padding: '8px',
                                        textDecoration: 'none',
                                        color: '#333',
                                    }}
                                    onClick={() => setShowDropdown(false)}
                                >
                                    <FontAwesomeIcon icon={faHome} />
                                    Перейти в админку
                                </Link>
                            ) : null}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default MyNavbar;