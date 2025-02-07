import React, { useState } from 'react';
import { Container, Card, Form, Button, Alert } from 'react-bootstrap';
import { NavLink, useNavigate } from 'react-router-dom';
import classNames from 'classnames/bind';

import { useAuth } from '~/contexts/AuthContext';
import * as authService from '~/services/authService';
import { getUserRole } from '~/utils/jwtUtils';
import styles from './Register.module.css';

const cx = classNames.bind(styles);
function Register() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [error, setError] = useState('');
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: '',
    address: '',
  });
  const handleChange = e => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };
  const handleSubmit = e => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      setError('Passwords do not match!');
      return;
    }
    const fetchApi = async () => {
      try {
        const res = await authService.register(formData);
        if (res) {
          const token = getUserRole(res.token);
          login({ username: formData.email, ...token });
          localStorage.setItem('token', res.token);
          navigate('/');
        } else setError(res.Errors);
      } catch (error) {
        if (error.response && error.response.status === 400) {
          setError(error.response.data[0] || 'Bad Request');
        } else {
          setError('Registration failed!');
        }
      }
    };
    fetchApi();
  };
  return (
    <Container className={cx('register__container')}>
      <Card className={cx('register__card')}>
        <Card.Body>
          <h2 className="text-center mb-4">Register</h2>
          {error && <Alert variant="danger">{error}</Alert>}
          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="firstName">
              <Form.Label>First Name</Form.Label>
              <Form.Control
                type="text"
                name="firstName"
                value={formData.firstName}
                onChange={handleChange}
                required
              />
            </Form.Group>
            <Form.Group controlId="lastName">
              <Form.Label>Last Name</Form.Label>
              <Form.Control
                type="text"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                required
              />
            </Form.Group>
            <Form.Group controlId="email" className="mt-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
              />
            </Form.Group>
            <Form.Group controlId="password" className="mt-3">
              <Form.Label>Password</Form.Label>
              <Form.Control
                type="password"
                name="password"
                value={formData.password}
                autoComplete="on"
                onChange={handleChange}
                required
              />
            </Form.Group>
            <Form.Group controlId="confirmPassword" className="mt-3">
              <Form.Label>Confirm Password</Form.Label>
              <Form.Control
                type="password"
                name="confirmPassword"
                value={formData.confirmPassword}
                autoComplete="on"
                onChange={handleChange}
                required
              />
            </Form.Group>
            <Button type="submit" className="mt-3 w-100" variant="primary">
              Register
            </Button>
          </Form>
          <div className="text-center mt-3">
            Already have an account? <NavLink to="/login">Login</NavLink>
          </div>
        </Card.Body>
      </Card>
    </Container>
  );
}

export default Register;
