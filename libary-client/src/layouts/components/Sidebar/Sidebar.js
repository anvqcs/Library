import { NavLink } from 'react-router-dom';
import classNames from 'classnames/bind';
import styles from './Sidebar.module.css';
import Menu, { MenuItem } from './Menu';
import config from '~/config';
const cx = classNames.bind(styles);

function Sidebar({ IsAdmin }) {
  return (
    <aside className={cx('wrapper', { wrapper__admin: IsAdmin })}>
      <div className={cx('header')}>
        <h2>
          <NavLink to={config.routes.home}>
            <span className={cx('header__title')}>Library</span>
          </NavLink>
        </h2>
      </div>
      <Menu>
        {!IsAdmin ? (
          <>
            <MenuItem title="Home" to={config.routes.home} />
            <MenuItem title="Transaction" to={config.routes.transaction} />
            <MenuItem title="Statistic" to={config.routes.statistic} />
          </>
        ) : (
          <>
            <MenuItem title="Book" to={config.routes.book} />
            <MenuItem title="User" to={config.routes.member} />
          </>
        )}
      </Menu>
    </aside>
  );
}

export default Sidebar;
