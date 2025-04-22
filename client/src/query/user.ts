import {gql} from '@apollo/client';


export const LOGIN_MUTATION = gql`
  mutation Login($email: String!, $password: String!) {
    login(email: $email, password: $password) {
      token
      user {
        id
        roles
      }
    }
  }
`

export const CHECK_AUTH_QUERY = gql`
  query CheckAuth {
    checkAuth {
      id
      roles
    }
  }
`

export const LOGOUT_MUTATION = gql`
  mutation Logout {
    logout
  }
`;