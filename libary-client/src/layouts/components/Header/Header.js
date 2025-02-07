import React from 'react';
import { NavLink } from 'react-router-dom';
import { Navbar, Nav, NavDropdown } from 'react-bootstrap';

import { useAuth } from '~/contexts/AuthContext';
import classNames from 'classnames/bind';
import styles from './Header.module.css';

const cx = classNames.bind(styles);

function Header() {
  const { user, logout } = useAuth();
  const handleLogout = () => {
    logout();
  };

  return (
    <Navbar expand="lg" className={cx('wrapper')}>
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav className="ms-auto">
          {user && (
            <NavDropdown
              title={`Hello, ${user.username}`}
              id="basic-nav-dropdown"
            >
              <NavDropdown.Item as={NavLink} to={`/user-form/${user.userId}`}>
                My Account
              </NavDropdown.Item>
              {user.role.includes('Administrator') && (
                <NavDropdown.Item as={NavLink} to="/members">
                  Administration
                </NavDropdown.Item>
              )}
              <NavDropdown.Item onClick={handleLogout}>Logout</NavDropdown.Item>
            </NavDropdown>
          )}
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
}

export default Header;
