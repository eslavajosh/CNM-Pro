import * as Yup from 'yup';

const changePassSchema = Yup.object().shape({
    password: Yup
      .string()
      .required("Please Enter your password")
      .matches(
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*])(?=.{8,})/,
        "Must Contain at least 8 Characters, One Uppercase, One Lowercase, One Number and One Special Case Character"
    )
});

export default changePassSchema;

