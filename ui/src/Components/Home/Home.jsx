import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from './Home.module.css'
import { Navbar } from '../Common/Navbar'

export const Home = () => {
    return (
        <div>
            <Navbar />
            <div className={styles.homeContainer}>
                <table>
					<tr>HI</tr>
				</table>
            </div>
        </div >
    );
};
