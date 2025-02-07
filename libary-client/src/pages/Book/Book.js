import { useState, useEffect } from 'react';
import { NavLink } from 'react-router-dom';
import { Button, Table } from 'react-bootstrap';
import classNames from 'classnames/bind';

import Pagination from '~/components/Pagination';
import DeleteModal from '~/components/DeleteModal';
import Search from './Search';
import * as bookService from '~/services/bookService';
import styles from './Book.module.css';

const cx = classNames.bind(styles);
function Book() {
  const [books, setBooks] = useState([]);
  const [filterData, setFilterData] = useState({});
  const [showModal, setShowModal] = useState(false);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedBook, setSelectedBook] = useState(null);
  const pageSize = 8;

  // Open confirmation modal
  const handleShowModal = Book => {
    setSelectedBook(Book);
    setShowModal(true);
  };

  // Close modal
  const handleCloseModal = () => {
    setShowModal(false);
    setSelectedBook(null);
  };

  // Delete Book function
  const handleDelete = () => {
    if (selectedBook) {
      const fetchApi = async () => {
        await bookService.remove(selectedBook.id);

        const result = await bookService.getWithPagination(
          currentPage,
          pageSize,
          {}
        );
        setBooks(result.items);
        setTotalPages(result.totalPages);
      };
      fetchApi();
    }
    handleCloseModal();
  };
  useEffect(() => {
    const fetchApi = async () => {
      const result = await bookService.getWithPagination(
        currentPage,
        pageSize,
        {
          ...filterData,
        }
      );
      setBooks(result.items);
      setTotalPages(result.totalPages);
    };
    fetchApi();
  }, [currentPage, filterData]);

  const handleSearch = data => {
    setCurrentPage(1);
    setFilterData(data);
  };
  return (
    <div className="mt-4">
      <h3 className="text-center mb-3">Library Books</h3>
      <div className={cx('search-bar', 'd-flex', 'gap-2')}>
        <Search onSubmit={handleSearch} />
        <NavLink to="/book-form">
          <Button className={cx('btn', 'btn--new')} variant="primary">
            Add New Book
          </Button>
        </NavLink>
      </div>
      <Table
        striped
        bordered
        hover
        responsive
        className={cx('table', 'shadow-sm')}
      >
        <thead>
          <tr className="bg-primary text-white">
            <th>#</th>
            <th>Title</th>
            <th>Author</th>
            <th>Genre</th>
            <th>Quantity</th>
            <th>Price</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {books &&
            books.map((book, index) => (
              <tr key={book.id}>
                <td>{(currentPage - 1) * pageSize + index + 1}</td>
                <td>{book.title}</td>
                <td>{book.author}</td>
                <td>{book.genre}</td>
                <td>{book.quantity}</td>
                <td>{book.rentalPrice}</td>
                <td>
                  <Button
                    className={cx('table__btn', 'btn__delete')}
                    variant="danger"
                    size="sm"
                    onClick={() => handleShowModal(book)}
                  >
                    Delete
                  </Button>
                  <NavLink to={'/book-form/' + book.id}>
                    <Button className={cx('table__btn', 'btn__edit')}>
                      Edit
                    </Button>
                  </NavLink>
                </td>
              </tr>
            ))}
        </tbody>
      </Table>
      <Pagination
        currentPage={currentPage}
        maxPageButtons={5}
        totalPages={totalPages}
        setCurrentPage={setCurrentPage}
      />

      <DeleteModal
        title={selectedBook && selectedBook.title}
        showModal={showModal}
        onCloseModal={handleCloseModal}
        onDelete={handleDelete}
      />
    </div>
  );
}

export default Book;
