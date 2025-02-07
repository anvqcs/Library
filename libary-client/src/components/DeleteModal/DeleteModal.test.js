import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import DeleteModal from './DeleteModal';

describe('DeleteModal', () => {
  const title = 'Test Item';
  const onCloseModal = jest.fn();
  const onDelete = jest.fn();

  beforeEach(() => {
    onCloseModal.mockClear();
    onDelete.mockClear();
  });
  const setup = showModal => {
    render(
      <DeleteModal
        title={title}
        showModal={showModal}
        onCloseModal={onCloseModal}
        onDelete={onDelete}
      />
    );
  };
  test('renders correctly when showModal is true', () => {
    setup(true);
    expect(screen.getByText('Confirm Deletion')).toBeInTheDocument();
    expect(screen.getByText('Cancel')).toBeInTheDocument();
    expect(screen.getByText('Delete')).toBeInTheDocument();
  });

  test('does not render when showModal is false', () => {
    setup(false);
    expect(screen.queryByText('Confirm Deletion')).not.toBeInTheDocument();
  });

  test('calls onCloseModal when Cancel button is clicked', () => {
    setup(true);
    fireEvent.click(screen.getByText('Cancel'));
    expect(onCloseModal).toHaveBeenCalledTimes(1);
  });

  test('calls onDelete when Delete button is clicked', () => {
    setup(true);
    fireEvent.click(screen.getByText('Delete'));
    expect(onDelete).toHaveBeenCalledTimes(1);
  });
});
