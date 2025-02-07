import { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Button, Form } from 'react-bootstrap';

import * as bookService from '~/services/bookService';
import * as genreService from '~/services/genreService';

function BookForm() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [genres, setGenres] = useState([]);
  const [book, setbook] = useState({
    title: '',
    author: '',
    year: '',
    isbn: '',
    description: '',
    quantity: '',
    genreId: '',
    rentalPrice: '',
  });

  useEffect(() => {
    const fetchApi = async () => {
      const result = await genreService.getAll();
      setGenres(result);
    };
    fetchApi();
  }, []);

  useEffect(() => {
    const fetchApi = async () => {
      const result = await bookService.getById(id);
      setbook(result);
    };
    if (id) fetchApi();
  }, [id]);
  const handleChange = e => {
    setbook({ ...book, [e.target.name]: e.target.value });
  };
  const handleSubmit = e => {
    e.preventDefault();
    if (id) {
      const fetchApi = async () => {
        const putData = {
          ...book,
        };
        await bookService.update(putData);
      };
      fetchApi();
    } else {
      const fetchApi = async () => {
        const postData = {
          ...book,
        };
        await bookService.add(postData);
      };
      fetchApi();
    }
    navigate('/books');
  };
  return (
    <div>
      <h3>{id ? 'Edit book' : 'Add New book'}</h3>
      <Form onSubmit={handleSubmit}>
        <Form.Group>
          <Form.Label>Title: </Form.Label>
          <Form.Control
            type="text"
            name="title"
            value={book.title}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Author: </Form.Label>
          <Form.Control
            type="text"
            name="author"
            value={book.author}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Year: </Form.Label>
          <Form.Control
            type="number"
            name="year"
            value={book.year}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>ISBN: </Form.Label>
          <Form.Control
            type="text"
            name="isbn"
            value={book.isbn}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Description: </Form.Label>
          <Form.Control
            type="text"
            name="description"
            value={book.description}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Quantity: </Form.Label>
          <Form.Control
            type="number"
            name="quantity"
            value={book.quantity}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Form.Group>
          <Form.Label>Genre: </Form.Label>
          <Form.Select
            name="genreId"
            value={book.genreId}
            onChange={handleChange}
          >
            <option value="">Select a genre</option>
            {genres.map(genre => (
              <option key={genre.id} value={genre.id}>
                {genre.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>
        <Form.Group>
          <Form.Label>RentalPrice: </Form.Label>
          <Form.Control
            type="number"
            name="rentalPrice"
            value={book.rentalPrice}
            onChange={handleChange}
          ></Form.Control>
        </Form.Group>
        <Button type="submit" variant="primary">
          {id ? 'Save Changes' : 'Add book'}
        </Button>
      </Form>
    </div>
  );
}

export default BookForm;
