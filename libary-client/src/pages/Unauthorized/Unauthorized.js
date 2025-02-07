import React from 'react';
import { Container, Button } from 'react-bootstrap';
import { useNavigate } from 'react-router-dom';
import styles from './Unauthorized.module.css';
import classNames from 'classnames/bind';

const cx = classNames.bind(styles);

function Unauthorized() {
  const navigate = useNavigate();

  return (
    <Container className={cx('unauthorizedContainer')}>
      <div className={cx('card')}>
        <h2>403 - Unauthorized</h2>
        <p>You do not have permission to access this page.</p>
        <Button variant="primary" onClick={() => navigate('/')}>
          Go to Home
        </Button>
      </div>
    </Container>
  );
}

export default Unauthorized;
