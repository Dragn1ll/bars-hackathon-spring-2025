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
`;

export const CHECK_AUTH_QUERY = gql`
  query CheckAuth {
    checkAuth {
      id
      roles
    }
  }
`;

export const LOGOUT_MUTATION = gql`
  mutation Logout {
    logout
  }
`;

export const GET_USERS_WITH_STATS = gql`
  query GetUsersWithStats($courseId: ID, $year: Int) {
    users {
      id
      name
      surname
      email
      role
      lastActive
      isActive: lastActive(format: "timestamp") @include(if: $includeActive)
      courses @include(if: $includeCourses) {
        id
        name
        score
        completed
      }
      stats @include(if: $includeStats) {
        totalCourses
        completedCourses
        averageScore
      }
    }
    analytics(year: $year) {
      monthlyActivity {
        month
        activeUsers
      }
      courseStats(courseId: $courseId) {
        excellent
        good
        average
        weak
      }
    }
  }
`;