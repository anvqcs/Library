import { jwtDecode } from 'jwt-decode';

export const getUserRole = token => {
  try {
    const decodedToken = jwtDecode(token);
    return {
      role:
        decodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ] || [],
      userId:
        decodedToken[
          'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
        ] || '',
      expiration: decodedToken.exp,
    };
  } catch (error) {
    console.error('Invalid token:', error);
    return null;
  }
};
