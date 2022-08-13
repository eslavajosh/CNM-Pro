import * as Yup from 'yup';

const forgotPassSchema = Yup.object().shape({
    email: Yup.string().email('Please enter a valid email.').required('An email is required.'),
});

export default forgotPassSchema;
