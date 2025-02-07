import * as httpRequest from '~/utils/httpRequest';

export const getAll = async (pageNumber, pageSize) => {
  try {
    const res = await httpRequest.get('users', {
      params: {
        pageNumber,
        pageSize,
      },
    });
    return res;
  } catch (error) {
    console.log(error);
  }
};
export const getById = async id => {
  try {
    const res = await httpRequest.get(`users/${id}`);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const add = async formData => {
  try {
    const res = await httpRequest.post('users', formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};

export const update = async formData => {
  try {
    const res = await httpRequest.put('users', formData);
    return res;
  } catch (error) {
    console.log(error);
  }
};
