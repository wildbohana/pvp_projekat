import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from './Home.module.css'
import { Navbar } from '../Common/Navbar'

import User from '../../Classes/User'
import Drive from '../../Classes/Drive'

export const Home = () => {
    return (
        <>
            <Navbar />
            <div className={styles.homeContainer}>
				<User />
				<hr/>
				<Drive/>
            </div>
        </>
    );
};
