import classNames from 'classnames/bind';
import PropTypes from 'prop-types';
import { NavLink } from 'react-router-dom';
import styles from './Menu.module.css';

const cx = classNames.bind(styles);

function MenuItem({ title, to }) {
  return (
    <NavLink
      className={(nav) => cx('menu-item', { active: nav.isActive })}
      to={to}
    >
      <span className={cx('title')}>{title}</span>
    </NavLink>
  );
}

MenuItem.propTypes = {
  title: PropTypes.string.isRequired,
  to: PropTypes.string.isRequired,
};
export default MenuItem;
