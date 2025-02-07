import React, { useState } from 'react';
import { Container, Form, Card, Button, Alert } from 'react-bootstrap';
import { useNavigate, NavLink } from 'react-router-dom';

import { useAuth } from '~/contexts/AuthContext';
import * as authService from '~/services/authService';
import { getUserRole } from '~/utils/jwtUtils';
import classNames from 'classnames/bind';
import styles from './Login.module.css';

const cx = classNames.bind(styles);

function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleLogin = e => {
    e.preventDefault();
    const fetchApi = async () => {
      try {
        const loginAccount = {
          email,
          password,
        };
        const res = await authService.login(loginAccount);
        if (res) {
          const token = getUserRole(res.token);
          login({ username: email, ...token });
          localStorage.setItem('token', res.token);
          navigate('/');
        }
      } catch (error) {
        setError('Invalid email or password');
      }
    };
    fetchApi();
  };
  return (
    <Container
      className={cx(
        'loginContainer',
        'd-flex align-items-center justify-content-center'
      )}
    >
      <Card className={cx('loginCard')}>
        <Card.Body>
          <h2 className="text-center mb-4">Login</h2>
          {error && <Alert variant="danger">{error}</Alert>}
          <Form onSubmit={handleLogin}>
            <Form.Group controlId="email">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                value={email}
                onChange={e => setEmail(e.target.value)}
                placeholder="Enter email"
                required
              />
            </Form.Group>

            <Form.Group controlId="password" className="mt-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                placeholder="Enter password"
                autoComplete="on"
                required
              />
            </Form.Group>
            <Button type="submit" className="mt-3 w-100" variant="primary">
              Login
            </Button>
            <NavLink to="/register">
              <Button className="mt-3 w-100" variant="primary">
                Register
              </Button>
            </NavLink>
          </Form>
        </Card.Body>
      </Card>
    </Container>
  );
}

export default Login;
