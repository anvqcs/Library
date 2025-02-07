import config from '~/config';

//Layouts
import { AdminLayout } from '~/layouts';
import { DefaultLayout } from '~/layouts';
import { BlankLayout } from '~/layouts';
//Pages
import Home from '~/pages/Home';
import Book from '~/pages/Book';
import BookForm from '~/pages/Book/BookForm';
import User from '~/pages/User';
import UserForm from '~/pages/User/UserForm';
import Setting from '~/pages/Setting';
import Transaction from '~/pages/Transaction';
import Statistic from '~/pages/Statistic';
import Login from '~/pages/Login';
import Unauthorized from '~/pages/Unauthorized';
import Register from '~/pages/Register';

const routes = [
  { path: config.routes.home, component: Home, layout: DefaultLayout },
  // Book
  { path: config.routes.book, component: Book, layout: AdminLayout },
  { path: config.routes.addBook, component: BookForm, layout: AdminLayout },
  { path: config.routes.editBook, component: BookForm, layout: AdminLayout },

  // User
  { path: config.routes.member, component: User, layout: AdminLayout },
  { path: config.routes.addUser, component: UserForm, layout: AdminLayout },
  {
    path: config.routes.editUser,
    component: UserForm,
    layout: AdminLayout,
  },

  { path: config.routes.setting, component: Setting, layout: AdminLayout },

  { path: config.routes.transaction, component: Transaction },

  { path: config.routes.statistic, component: Statistic },

  { path: config.routes.login, component: Login, layout: BlankLayout },
  { path: config.routes.register, component: Register, layout: BlankLayout },
  {
    path: config.routes.unauthorized,
    component: Unauthorized,
    layout: BlankLayout,
  },
];

export { routes };
