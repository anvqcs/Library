import { useEffect, useState } from 'react';
import { useParams, useNavigate, Navigate } from 'react-router-dom';
import { Button, Form } from 'react-bootstrap';

import { useAuth } from '~/contexts/AuthContext';
import * as applicationUserService from '~/services/applicationUserService';

function UserForm() {
  const { user } = useAuth();
  const { id } = useParams();
  const navigate = useNavigate();
  const [member, setUser] = useState({
    userName: '',
    firstName: '',
    lastName: '',
    phoneNumber: '',
    address: '',
  });
  useEffect(() => {
    const fetchApi = async () => {
      const result = await applicationUserService.getById(id);
      setUser(result);
    };

    fetchApi();
  }, [id]);
  const handleChange = e => {
    setUser({ ...member, [e.target.name]: e.target.value });
  };
  if (!user.role.includes('Administrator') && user.userId !== id) {
    return <Navigate to="/unauthorized" />;
  }
  const handleSubmit = e => {
    e.preventDefault();
    if (id) {
      const fetchApi = async () => {
        const putData = {
          id,
          ...member,
        };
        await applicationUserService.update(putData);
      };
      fetchApi();
    } else {
      const fetchApi = async () => {
        const postData = {
          ...member,
        };
        await applicationUserService.add(postData);
      };
      fetchApi();
    }
    navigate('/members');
  };
  return (
    <div>
      <h3>{id ? 'Edit User' : 'Add New User'}</h3>
      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label>Username: </Form.Label>
          <Form.Control
            type="text"
            name="userName"
            value={member.userName}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>First Name: </Form.Label>
          <Form.Control
            type="text"
            name="firstName"
            value={member.firstName}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Last Name: </Form.Label>
          <Form.Control
            type="text"
            name="lastName"
            value={member.lastName}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Phone Number: </Form.Label>
          <Form.Control
            type="text"
            name="phoneNumber"
            value={member.phoneNumber}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Address: </Form.Label>
          <Form.Control
            type="text"
            name="address"
            value={member.address}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Button type="submit" variant="primary">
          {id ? 'Save Changes' : 'Add User'}
        </Button>
      </Form>
    </div>
  );
}

export default UserForm;
