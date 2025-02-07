import * as httpRequest from '~/utils/httpRequest';

export const login = async data => {
  const res = await httpRequest.post('auth/login', data);
  return res;
};

export const register = async data => {
  const res = await httpRequest.post('auth/register', data);
  return res;
};
