// Navbar.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './Navbar.module.css';
import axios from 'axios';

export const Navbar = () => {
    return (
        <header>
            <nav>
                <ul className={styles.nav_links}>
                    <li>
                        <a href='/home'>Home</a>
                    </li>  
                </ul>
            </nav>
        </header>
    );
};
